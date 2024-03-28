namespace RollerCoaster.Services;

public interface IPasswordHashService
{
    public string GenerateHash(string password);
}