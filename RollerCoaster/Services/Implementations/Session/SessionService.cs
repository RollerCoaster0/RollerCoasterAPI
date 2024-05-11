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
        throw new NotImplementedException();
    }

    public Task<int> Create(int creatorId, SessionCreationDTO roomCreationDto)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(int accessorId, int id)
    {
        var session = await dataBaseContext.Sessions.FindAsync(id);
        
        if (session is null)
            throw new NotFoundError("Сессия не найдена.");

        if (session.GameMasterId != accessorId)
            throw new AccessDeniedError("У вас нет доступа к этой сессии.");
        
        dataBaseContext.Remove(session);
        await dataBaseContext.SaveChangesAsync();
    }

    public Task Join(int userId, PlayerCreationDTO playerCreationDto)
    {
        throw new NotImplementedException();
    }

    public Task Start(int userId)
    {
        throw new NotImplementedException();
    }
}