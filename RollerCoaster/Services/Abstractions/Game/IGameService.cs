using RollerCoaster.DataTransferObjects.Game;

namespace RollerCoaster.Services.Abstractions.Game;

public interface IGameService
{
    Task<GameDTO> Get(int id);
    
    Task<int> Create(int accessorUserId, GameCreationDTO gameCreationDto);

    Task Delete(int accessorUserId, int id);
}