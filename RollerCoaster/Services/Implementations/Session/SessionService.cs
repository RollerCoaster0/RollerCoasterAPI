using Microsoft.EntityFrameworkCore;
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

        return new SessionDTO()
        {
            Id = session.Id,
            Name = session.Name,
            Description = session.Description,
            CreationDate = DateTimeOffset.Now,
            CreatorId = session.CreatorId
        };
    }

    public async Task<int> Create(int creatorId, SessionCreationDTO sessionCreationDto)
    {
        throw new NotImplementedException();
    }
    
    public async Task Delete(int accessorId, int id)
    {
        var session = await dataBaseContext.Sessions.FindAsync(id);
        
        if (session is null)
            throw new NotFoundError("Сессия не найдена.");

        if (session.CreatorId != accessorId)
            throw new AccessDeniedError("У вас нет доступа к этой сессии.");
        
        dataBaseContext.Remove(session);
        await dataBaseContext.SaveChangesAsync();
    }
}