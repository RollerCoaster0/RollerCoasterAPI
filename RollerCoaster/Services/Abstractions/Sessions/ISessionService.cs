using RollerCoaster.DataTransferObjects.Session;
using RollerCoaster.DataTransferObjects.Session.Locations;

namespace RollerCoaster.Services.Abstractions.Sessions;

public interface ISessionService
{
    Task<SessionDTO> Get(int accessorUserId, int sessionId);

    Task<int> Create(int creatorUserId, SessionCreationDTO sessionCreationDto);

    Task Delete(int accessorUserId, int sessionId);
    
    Task Start(int accessorUserId, int sessionId);

    Task ChangeLocation(int accessorUserId, int sessionId, ChangeLocationDTO locationDto);
}