using RollerCoaster.DataTransferObjects.Session;

namespace RollerCoaster.Services.Abstractions.Sessions;

public interface ISessionService
{
    Task<SessionDTO> Get(int accessorUserId, int sessionId);

    Task<int> Create(int creatorId, SessionCreationDTO sessionCreationDto);

    Task Delete(int accessorUserId, int sessionId);
    
    public Task Start(int accessorUserId, int sessionId);
}