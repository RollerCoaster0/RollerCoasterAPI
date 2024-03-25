using RollerCoaster.DataTransferObjects;

namespace RollerCoaster.Services;

public class UsersService(DataBaseContext dataBaseContext): IUsersService
{
    public async Task<GetMeDTO?> GetMe(int id)
    {
        var user = await dataBaseContext.Users.FindAsync(id);
        
        if (user is null)
            return null;
        
        return new GetMeDTO
        {
            UserId = user.Id,
            Login = user.Login
        };
    }

    public async Task<UserDTO?> GetUser(int id)
    {
        var user = await dataBaseContext.Users.FindAsync(id);

        if (user is null)
            return null;
        
        return new UserDTO
        {
            UserId = user.Id
        };
    }
}