using RollerCoaster.DataTransferObjects.Users;
using RollerCoaster.DataTransferObjects.Users.Auth;

namespace RollerCoaster.Services.Abstractions.Users;

public struct AuthorizedUserMeta
{
    public required int Id { get; set; }
    public required string Token { get; set; }
}

public interface IAuthService
{
    public Task<AuthorizedUserMeta> Register(RegisterDTO registerDto);
    
    public Task<AuthorizedUserMeta> Login(LoginDTO loginDto);
}