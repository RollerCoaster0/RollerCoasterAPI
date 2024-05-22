using RollerCoaster.DataBase;
using RollerCoaster.DataTransferObjects.Session;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Services.Implementations.Session;

public class SessionService(DataBaseContext dataBaseContext) : ISessionService
{
    // TODO: в начавшейся игре не участнки не могут запрашивать сессию
    
    public async Task<SessionDTO> Get(int sessionId)
    {
        var session = await dataBaseContext.Sessions
            .FindAsync(sessionId);

        if (session is null)
        {
            throw new NotFoundError("Сессия не найдена");
        }

        return new SessionDTO
        {
            Id = session.Id,
            Name = session.Name,
            Description = session.Description,
            GameMasterId = session.GameMasterId,
            GameId = session.GameId,
            Players = [], // empty list for now
            IsActive = session.IsActive
        };
    }

    public async Task<int> Create(int creatorId, SessionCreationDTO roomCreationDto)
    {
        var game = await dataBaseContext.Games.FindAsync(roomCreationDto.GameId);

        if (game is null)
            throw new NotFoundError("Игра не найдена.");

        if (game.BaseLocationId is null)
            throw new ProvidedDataIsInvalidError("Игра редактируется.");
        
        // TODO: проверить, что у всех локаций есть карта

        var session = new DataBase.Models.Session.Session
        {
            Name = roomCreationDto.Name,
            Description = roomCreationDto.Description,
            CurrentPlayersLocationId = game.BaseLocationId.Value,
            GameId = roomCreationDto.GameId,
            GameMasterId = creatorId,
            IsActive = false,
            Players = []
        };

        await dataBaseContext.AddAsync(session);
        await dataBaseContext.SaveChangesAsync();

        return session.Id;
    }

    public async Task Delete(int accessorUserId, int sessionId)
    {
        var session = await dataBaseContext.Sessions.FindAsync(sessionId);
        
        // TODO: тут надо каскадно и остальные части удалять
        
        if (session is null)
            throw new NotFoundError("Сессия не найдена.");

        if (session.GameMasterId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этой сессии.");
        
        dataBaseContext.Remove(session);
        await dataBaseContext.SaveChangesAsync();
    }

    public async Task Start(int accessorUserId, int sessionId)
    {
        var session = await dataBaseContext.Sessions.FindAsync(sessionId);
        
        if (session is null)
            throw new NotFoundError("Сессия не найдена.");
        
        if (session.GameMasterId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этой сессии.");

        session.IsActive = true;
        await dataBaseContext.SaveChangesAsync();
    }
}