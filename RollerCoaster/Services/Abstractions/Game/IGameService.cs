using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.DataTransferObjects.Game.Fetching;

namespace RollerCoaster.Services.Abstractions.Game;

public interface IGameService
{
    Task<GameDTO> Get(int id);
    
    Task<int> Create(int creatorId, GameCreationDTO gameCreationDto);

    Task Delete(int accessorId, int id);
}