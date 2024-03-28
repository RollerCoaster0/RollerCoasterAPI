using RollerCoaster.DataTransferObjects.Users;

namespace RollerCoaster.Services;

public interface IUsersService
{
    Task<GetMeDTO?> GetMe(int id);
    
    Task<UserDTO?> GetUser(int id);
}