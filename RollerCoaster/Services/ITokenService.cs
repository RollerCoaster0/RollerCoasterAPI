namespace RollerCoaster.Services;

public interface ITokenService
{
    public string GenerateToken(int userId);
}