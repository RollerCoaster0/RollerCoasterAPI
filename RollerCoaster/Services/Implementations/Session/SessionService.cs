using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Session;
using RollerCoaster.DataTransferObjects.LongPoll;
using RollerCoaster.DataTransferObjects.Session;
using RollerCoaster.DataTransferObjects.Session.Locations;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.LongPoll;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Services.Implementations.Session;

public class SessionService(
    DataBaseContext dataBaseContext,
    ILongPollService longPollService) : ISessionService
{
    public async Task<SessionDTO> Get(int accessorUserId, int sessionId)
    {
        var session = await dataBaseContext.Sessions
            .Include(s => s.Players)
            .FirstOrDefaultAsync(s => s.Id == sessionId);

        if (session is null)
            throw new NotFoundError("Сессия не найдена");
        
        var isUserMemberOfSession = await dataBaseContext.Players
            .Where(p => p.SessionId == session.Id && p.UserId == accessorUserId)
            .AnyAsync();
        
        var isUserGameMasterOfSession = session.GameMasterUserId == accessorUserId;
        
        if (!isUserMemberOfSession && !isUserGameMasterOfSession && session.IsActive)
            throw new AccessDeniedError("У вас нет доступа к этой сессии.");

        return new SessionDTO
        {
            Id = session.Id,
            Name = session.Name,
            Description = session.Description,
            GameMasterUserId = session.GameMasterUserId,
            GameId = session.GameId,
            IsActive = session.IsActive,
            CurrentPlayersLocationId = session.CurrentPlayersLocationId
        };
    }

    public async Task<int> Create(int creatorUserId, SessionCreationDTO roomCreationDto)
    {
        var game = await dataBaseContext.Games
            .Include(g => g.Locations)
            .Include(g => g.NonPlayableCharacters)
            .FirstOrDefaultAsync(g => g.Id == roomCreationDto.GameId);

        if (game is null)
            throw new NotFoundError("Игра не найдена.");
    
        if (game.BaseLocationId is null)
            throw new ProvidedDataIsInvalidError("Игра редактируется.");
        
        foreach (var location in game.Locations)
        {
            if (location.MapFilePath is null)
                throw new ProvidedDataIsInvalidError("Игра редактируется.");
        }

        var session = new DataBase.Models.Session.Session
        {
            Name = roomCreationDto.Name,
            Description = roomCreationDto.Description,
            CurrentPlayersLocationId = game.BaseLocationId.Value,
            GameId = roomCreationDto.GameId,
            GameMasterUserId = creatorUserId,
            IsActive = false
        };

        await dataBaseContext.AddAsync(session);
        await dataBaseContext.SaveChangesAsync();
        
        foreach (var npc in game.NonPlayableCharacters)
        {
            var anpc = new ActiveNonPlayableCharacter
            {
                NonPlayableCharacterId = npc.Id,
                SessionId = session.Id,
                HealthPoints = 100,
                CurrentXPosition = npc.BaseXPosition,
                CurrentYPosition = npc.BaseYPosition
            };
            await dataBaseContext.ActiveNonPlayableCharacters.AddAsync(anpc);
        }
        await dataBaseContext.SaveChangesAsync();
        
        var update = new SessionDTO
        {
            Id = session.Id,
            Name = session.Name,
            Description = session.Description,
            GameMasterUserId = session.GameMasterUserId,
            GameId = session.GameId,
            CurrentPlayersLocationId = session.CurrentPlayersLocationId,
            IsActive = session.IsActive
        };

        var membersOfSessionUserIds = await dataBaseContext.Players
            .Where(p => p.SessionId == session.Id)
            .Select(p => p.UserId)
            .ToListAsync();
        membersOfSessionUserIds.Add(session.GameMasterUserId);

        var tasks = new List<Task>();
        foreach (var userId in membersOfSessionUserIds)
        {
            Task task = longPollService.EnqueueUpdateAsync(userId, new LongPollUpdate
            {
                QuestStatusUpdate = null,
                NewMessage = null,
                Move = null,
                SessionStarted = update
            });
            tasks.Add(task);
        }
        await Task.WhenAll(tasks);

        return session.Id;
    }

    public async Task Delete(int accessorUserId, int sessionId)
    {
        var session = await dataBaseContext.Sessions.FindAsync(sessionId);
        
        // TODO: тут надо каскадно и остальные части удалять
        
        if (session is null)
            throw new NotFoundError("Сессия не найдена.");

        if (session.GameMasterUserId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этой сессии.");
        
        dataBaseContext.Remove(session);
        await dataBaseContext.SaveChangesAsync();
    }

    public async Task Start(int accessorUserId, int sessionId)
    {
        var session = await dataBaseContext.Sessions.FindAsync(sessionId);
        
        if (session is null)
            throw new NotFoundError("Сессия не найдена.");
        
        if (session.GameMasterUserId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этой сессии.");

        session.IsActive = true;
        await dataBaseContext.SaveChangesAsync();
    }

    public async Task ChangeLocation(int accessorUserId, int sessionId, ChangeLocationDTO locationDto)
    {
        var session = await dataBaseContext.Sessions.FindAsync(sessionId);

        if (session is null)
            throw new NotFoundError("Сессия не найдена.");

        if (session.GameMasterUserId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этой сессии.");

        if (session.CurrentPlayersLocationId == locationDto.LocationId)
            throw new ProvidedDataIsInvalidError("Выберите отличную от текущей локацию.");
        
        var location = await dataBaseContext.Locations.FindAsync(locationDto.LocationId);

        if (location is null)
            throw new NotFoundError("Данная локация не найдена.");

        if (location.GameId != session.GameId)
            throw new ProvidedDataIsInvalidError("Данная локация принадлежит другой игре.");

        if (!session.IsActive)
            throw new ProvidedDataIsInvalidError("Игра не началась.");
        
        session.CurrentPlayersLocationId = locationDto.LocationId;
        await dataBaseContext.SaveChangesAsync();
    }
}