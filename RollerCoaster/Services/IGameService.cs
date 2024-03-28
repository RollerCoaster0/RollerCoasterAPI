using RollerCoaster.DataTransferObjects.Game;

namespace RollerCoaster.Services;

public interface IGameService
{
    Task<GameDTO?> Get(int id);
    
    Task<int> Create(GameDTO game);

    Task Delete(int id);
}