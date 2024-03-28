using RollerCoaster.DataTransferObjects.Game;

namespace RollerCoaster.Services;

public interface IGameService
{
    Task<CreateGameDTO?> Get(int id);
    
    Task<int> Create(CreateGameDTO game);

    Task Delete(int id);
}