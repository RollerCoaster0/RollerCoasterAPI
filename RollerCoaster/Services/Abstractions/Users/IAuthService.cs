using RollerCoaster.DataTransferObjects.Users;

namespace RollerCoaster.Services.Abstractions.Users;

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