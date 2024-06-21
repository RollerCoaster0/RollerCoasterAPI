using RollerCoaster.DataTransferObjects.Users;

namespace RollerCoaster.Services.Abstractions.Users;

public interface IUsersService
{
    Task<GetMeDTO> GetMe(int accessorUserId);
    
    Task<UserDTO> GetUser(int userId);
}