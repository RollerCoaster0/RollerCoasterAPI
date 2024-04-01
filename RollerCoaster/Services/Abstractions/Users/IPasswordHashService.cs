namespace RollerCoaster.Services.Abstractions.Users;

public interface IPasswordHashService
{
    public string GenerateHash(string password);
}