using RollerCoaster.DataBase;
using RollerCoaster.DataTransferObjects.Users;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Users;

namespace RollerCoaster.Services.Realisations.Users;

public class UsersService(DataBaseContext dataBaseContext): IUsersService
{
    public async Task<GetMeDTO> GetMe(int id)
    {
        var user = await dataBaseContext.Users.FindAsync(id);
        
        if (user is null)
            throw new NotFoundError("Пользователь не найден.");
        
        return new GetMeDTO
        {
            UserId = user.Id,
            Login = user.Login
        };
    }

    public async Task<UserDTO> GetUser(int id)
    {
        var user = await dataBaseContext.Users.FindAsync(id);

        if (user is null)
            throw new NotFoundError("Пользователь не найден.");
        
        return new UserDTO
        {
            UserId = user.Id
        };
    }
}