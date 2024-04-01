using RollerCoaster.DataTransferObjects.Users;

namespace RollerCoaster.Services.Abstractions.Users;

public interface IUsersService
{
    Task<GetMeDTO> GetMe(int id);
    
    Task<UserDTO> GetUser(int id);
}