namespace RollerCoaster.Services.Abstractions.Users;

public interface ITokenService
{
    public string GenerateToken(int userId);
}