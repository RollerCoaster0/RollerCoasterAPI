using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Session;
using RollerCoaster.DataTransferObjects.Session;
using RollerCoaster.DataTransferObjects.Session.Players;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Services.Implementations.Session;

public class SessionService(DataBaseContext dataBaseContext) : ISessionService
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
            GameMasterId = session.GameMasterUserId,
            GameId = session.GameId,
            IsActive = session.IsActive,
            Players = session.Players.Select(p => new PlayerDTO
            {
                Id = p.Id,
                UserId = p.UserId,
                SessionId = p.SessionId,
                Name = p.Name,
                CharacterClassId = p.CharacterClassId,
                CurrentXPosition = p.CurrentXPosition,
                CurrentYPosition = p.CurrentYPosition,
                HealthPoints = p.HealthPoints,
                Level = p.Level
            }).ToList()
        };
    }

    public async Task<int> Create(int creatorId, SessionCreationDTO roomCreationDto)
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
            GameMasterUserId = creatorId,
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
}