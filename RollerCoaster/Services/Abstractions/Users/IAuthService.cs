using RollerCoaster.DataTransferObjects.Users;

namespace RollerCoaster.Services.Abstractions.Users;

public class InvalidLoginCredentialsError(string message): Exception(message);

public class LoginAlreadyInUseError(string message): Exception(message);

public struct CreatedUserMeta
{
    public required int UserId { get; set; }
    public required string AccessToken { get; set; }
}

public interface IAuthService
{
    public Task<CreatedUserMeta> Register(RegisterDTO registerDto);
    
    public Task<string> Login(LoginDTO loginDto);
}