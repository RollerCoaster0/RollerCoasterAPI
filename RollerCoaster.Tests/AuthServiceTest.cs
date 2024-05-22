using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using RollerCoaster.DataBase;
using RollerCoaster.DataTransferObjects.Users;
using RollerCoaster.DataTransferObjects.Users.Auth;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Implementations.Users;

namespace RollerCoaster.Tests;

[TestClass]
public class AuthServiceTest()
{
    private readonly SiteConfiguration.JWTConfiguration _jwt = new()
    {
        Audience = "fasdok",
        Issuer = "vcfnk",
        Key = "fsdoijsdfffdseytdrdhojkcfhdgxljkjkcgljkvcnbljkvcblk"
    };

    private readonly PasswordHashService _passwordHashService = new();

    [TestMethod]
    [ExpectedException(typeof(ProvidedDataIsInvalidError))]
    public async Task RegisterWithInvalidLogin()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;

        using var context = new DataBaseContext(options);

        var invalidNameRegisterDto = new RegisterDTO
        {
            Login = "a",
            Password = "cxboi45lhojkfsdSh23@"
        };


        var authService = new AuthService(context, new TokenService(Options.Create(_jwt)), _passwordHashService);

        await authService.Register(invalidNameRegisterDto);
    }

    [TestMethod]
    [ExpectedException(typeof(ProvidedDataIsInvalidError))]
    public async Task RegisterWithInvalidPassword()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;

        using var context = new DataBaseContext(options);

        var invalidPasswordRegisterDto = new RegisterDTO
        {
            Login = "oihfasdlnk",
            Password = "123"
        };

        var authService = new AuthService(context, new TokenService(Options.Create(_jwt)), _passwordHashService);

        await authService.Register(invalidPasswordRegisterDto);
    }


    [TestMethod]
    [ExpectedException(typeof(ProvidedDataIsInvalidError))]
    public async Task RegisterWithExistingName()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;

        using var context = new DataBaseContext(options);

        var registeredUser = new RegisterDTO
        {
            Login = "oihfasdlnk",
            Password = "cxboi45lhojkfsdSh23@"
        };
        
        var authService = new AuthService(context, new TokenService(Options.Create(_jwt)), _passwordHashService);

        await authService.Register(registeredUser);
        await authService.Register(registeredUser);
    }
    
    [TestMethod]
    [ExpectedException(typeof(AccessDeniedError))]
    public async Task LoginWithUnknownName()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;

        using var context = new DataBaseContext(options);

        var user = new LoginDTO
        {
            Login = "oihfasdlnk",
            Password = "cxboi45lhojkfsdSh23@"
        };
        
        var authService = new AuthService(context, new TokenService(Options.Create(_jwt)), _passwordHashService);

        await authService.Login(user);
    }
    
    [TestMethod]
    [ExpectedException(typeof(AccessDeniedError))]
    public async Task LoginWithIncorrectPassword()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;

        using var context = new DataBaseContext(options);

        var user = new RegisterDTO
        {
            Login = "oihfasdlnk",
            Password = "cxboi45lhojkfsdSh23@"
        };
        
        var authService = new AuthService(context, new TokenService(Options.Create(_jwt)), _passwordHashService);

        await authService.Register(user);

        var incorrectUser = new LoginDTO
        {
            Login = "oihfasdlnk",
            Password = "aaaa"
        };
        await authService.Login(incorrectUser);
    }

    [TestMethod]
    public async Task CorrectRegisterAndLogin()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;

        using var context = new DataBaseContext(options);

        var user = new RegisterDTO
        {
            Login = "oihfasdlnk",
            Password = "cxboi45lhojkfsdSh23@"
        };
        
        var authService = new AuthService(context, new TokenService(Options.Create(_jwt)), _passwordHashService);

        await authService.Register(user);

        var login = new LoginDTO
        {
            Login = "oihfasdlnk",
            Password = "cxboi45lhojkfsdSh23@"
        };
        await authService.Login(login);
    }
}