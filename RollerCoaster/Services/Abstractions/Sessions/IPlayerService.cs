using RollerCoaster.DataTransferObjects.Session.Creation;
using RollerCoaster.DataTransferObjects.Session.Fetching;

namespace RollerCoaster.Services.Abstractions.Sessions;

public interface IPlayerService
{
    public Task<PlayerDTO> Get(int accessorId, int playerId);
    public Task<int> Create(int accessorId, PlayerCreationDTO playerCreationDto);
    
}