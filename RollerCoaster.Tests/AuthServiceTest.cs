using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RollerCoaster.DataBase;
using RollerCoaster.DataTransferObjects.Users.Auth;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Implementations.Users;

namespace RollerCoaster.Tests;

[TestClass]
public class AuthServiceTest
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
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseSqlite(connection)
            .Options;
        await using var context = new DataBaseContext(options);
        
        var tokenService = new TokenService(Options.Create(_jwt));
        var authService = new AuthService(context, tokenService, _passwordHashService);
        
        var invalidNameRegisterDto = new RegisterDTO
        {
            Login = "a",
            Password = "cxboi45lhojkfsdSh23@"
        };
        await authService.Register(invalidNameRegisterDto);
    }

    [TestMethod]
    [ExpectedException(typeof(ProvidedDataIsInvalidError))]
    public async Task RegisterWithInvalidPassword()
    {
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseSqlite(connection)
            .Options;
        await using var context = new DataBaseContext(options);
        
        var tokenService = new TokenService(Options.Create(_jwt));
        var authService = new AuthService(context, tokenService, _passwordHashService);

        var invalidPasswordRegisterDto = new RegisterDTO
        {
            Login = "oihfasdlnk",
            Password = "123"
        };
        await authService.Register(invalidPasswordRegisterDto);
    }

    [TestMethod]
    [ExpectedException(typeof(ProvidedDataIsInvalidError))]
    public async Task RegisterWithExistingName()
    {
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseSqlite(connection)
            .Options;
        await using var context = new DataBaseContext(options);
        
        var tokenService = new TokenService(Options.Create(_jwt));
        var authService = new AuthService(context, tokenService, _passwordHashService);

        var registeredUser = new RegisterDTO
        {
            Login = "oihfasdlnk",
            Password = "cxboi45lhojkfsdSh23@"
        };
        await authService.Register(registeredUser);
        await authService.Register(registeredUser);
    }
    
    [TestMethod]
    [ExpectedException(typeof(AccessDeniedError))]
    public async Task LoginWithUnknownName()
    {
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseSqlite(connection)
            .Options;
        await using var context = new DataBaseContext(options);
        
        var tokenService = new TokenService(Options.Create(_jwt));
        var authService = new AuthService(context, tokenService, _passwordHashService);
        
        var user = new LoginDTO
        {
            Login = "oihfasdlnk",
            Password = "cxboi45lhojkfsdSh23@"
        };
        await authService.Login(user);
    }
    
    [TestMethod]
    [ExpectedException(typeof(AccessDeniedError))]
    public async Task LoginWithIncorrectPassword()
    {
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseSqlite(connection)
            .Options;
        await using var context = new DataBaseContext(options);

        var tokenService = new TokenService(Options.Create(_jwt));
        var authService = new AuthService(context, tokenService, _passwordHashService);

        var user = new RegisterDTO
        {
            Login = "oihfasdlnk",
            Password = "cxboi45lhojkfsdSh23@"
        };
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
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseSqlite(connection)
            .Options;
        await using var context = new DataBaseContext(options);
        
        var tokenService = new TokenService(Options.Create(_jwt));
        var authService = new AuthService(context, tokenService, _passwordHashService);
        
        var user = new RegisterDTO
        {
            Login = "oihfasdlnk",
            Password = "cxboi45lhojkfsdSh23@"
        };
        await authService.Register(user);

        var login = new LoginDTO
        {
            Login = "oihfasdlnk",
            Password = "cxboi45lhojkfsdSh23@"
        };
        await authService.Login(login);
    }
}