using RollerCoaster.DataTransferObjects.Users;

namespace RollerCoaster.Services.Abstractions.Users;

public interface IUsersService
{
    Task<GetMeDTO> GetMe(int userId);
    
    Task<UserDTO> GetUser(int userId);
}