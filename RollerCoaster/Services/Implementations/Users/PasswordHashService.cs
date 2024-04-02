using System.Security.Cryptography;
using System.Text;
using RollerCoaster.Services.Abstractions.Users;

namespace RollerCoaster.Services.Realisations.Users;

public class PasswordHashService : IPasswordHashService
{
    private readonly HashAlgorithm _hashAlgorithm = SHA256.Create();

    public string GenerateHash(string password)
    {
        var passwordAsBytes = Encoding.UTF8.GetBytes(password);
        return Convert.ToBase64String(_hashAlgorithm.ComputeHash(passwordAsBytes));
    }
}