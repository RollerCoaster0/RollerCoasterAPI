using RollerCoaster.DataBase;
using RollerCoaster.DataTransferObjects.Session.Creation;
using RollerCoaster.DataTransferObjects.Session.Fetching;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Services.Realisations.Session;

public class SessionService(DataBaseContext dataBaseContext) : ISessionService
{
    public async Task<SessionDTO> Get(int id)
    {
        var session = await dataBaseContext.Sessions
            .FindAsync(id);

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

    public Task<int> Create(int creatorId, SessionCreationDTO roomCreationDto)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(int accessorUserId, int id)
    {
        var session = await dataBaseContext.Sessions.FindAsync(id);
        
        if (session is null)
            throw new NotFoundError("Сессия не найдена.");

        if (session.GameMasterId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этой сессии.");
        
        dataBaseContext.Remove(session);
        await dataBaseContext.SaveChangesAsync();
    }

    public Task Join(int userId, PlayerCreationDTO playerCreationDto)
    {
        throw new NotImplementedException();
    }

    public Task Start(int accessorUserId)
    {
        throw new NotImplementedException();
    }
}