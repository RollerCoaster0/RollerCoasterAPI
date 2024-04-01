using RollerCoaster.DataTransferObjects.Session.Creation;
using RollerCoaster.DataTransferObjects.Session.Fetching;

namespace RollerCoaster.Services.Abstractions.Sessions;

public interface ISessionService
{
    Task<SessionDTO> Get(int id);

    Task<int> Create(int creatorId, SessionCreationDTO sessionCreationDto);

    Task Delete(int accessorId, int id);
}