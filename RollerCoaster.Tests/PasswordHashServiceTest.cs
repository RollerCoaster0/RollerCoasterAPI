using RollerCoaster.Services.Realisations.Users;

namespace RollerCoaster.Tests;

[TestClass]
public class PasswordHashServiceTest
{
    private readonly PasswordHashService _passwordHashService = new();

    [TestMethod]
    public void HashGeneration()
    {
        string password = "cxboi45lhojkfsdSh23@";
        string samePassword = "cxboi45lhojkfsdSh23@";

        Assert.AreEqual(_passwordHashService.GenerateHash(password),
            _passwordHashService.GenerateHash(samePassword));
    }

    [TestMethod]
    public void DifferentPasswords()
    {
        string password = "cxboi45lhojkfsdSh23@";
        string otherPassword = "cxboi45hojkfsdSh23@";

        Assert.AreNotEqual(_passwordHashService.GenerateHash(password),
            _passwordHashService.GenerateHash(otherPassword));
    }
}