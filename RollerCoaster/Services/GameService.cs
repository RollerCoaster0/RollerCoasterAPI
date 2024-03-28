using RollerCoaster.DataBase;
using RollerCoaster.DataTransferObjects.Game;

namespace RollerCoaster.Services;

public class GameService (DataBaseContext dataBaseContext) : IGameService
{
    public async Task<GameDTO?> Get(int id)
    {
        var game = await dataBaseContext.Games.FindAsync(id);
        
        if (game is null)
            return null;
        throw new NotImplementedException();
        
    }

    public Task<int> Create(GameDTO game)
    {
        throw new NotImplementedException();
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }
}