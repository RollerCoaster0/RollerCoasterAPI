using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Implementations.Users;

namespace RollerCoaster.Tests;

[TestClass]
public class UsersServiceTest
{
    private readonly User _user = new()
    {
        Login = "oihfasdlnk",
        PasswordHash = "123"
    };

    [TestMethod]
    public async Task GetMeCorrect()
    {
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseSqlite(connection)
            .Options;
        await using var context = new DataBaseContext(options);

        await context.Users.AddAsync(_user);
        await context.SaveChangesAsync();

        var usersService = new UsersService(context);
        await usersService.GetMe(1);
    }

    [TestMethod]
    public async Task GetUserCorrect()
    {
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseSqlite(connection)
            .Options;
        await using var context = new DataBaseContext(options);

        await context.Users.AddAsync(_user);
        await context.SaveChangesAsync();

        var usersService = new UsersService(context);
        await usersService.GetMe(1);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundError))]
    public async Task GetMeNotFoundId()
    {
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseSqlite(connection)
            .Options;
        await using var context = new DataBaseContext(options);

        var usersService = new UsersService(context);
        await usersService.GetMe(1);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundError))]
    public async Task GetUserNotFoundId()
    {
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseSqlite(connection)
            .Options;
        await using var context = new DataBaseContext(options);

        var usersService = new UsersService(context);
        await usersService.GetUser(1);
    }
}