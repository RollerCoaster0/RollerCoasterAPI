using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;
        await using var context = new DataBaseContext(options);

        await context.Users.AddAsync(_user);

        var usersService = new UsersService(context);
        await usersService.GetMe(1);
    }

    [TestMethod]
    public async Task GetUserCorrect()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;
        await using var context = new DataBaseContext(options);

        await context.Users.AddAsync(_user);

        var usersService = new UsersService(context);
        await usersService.GetMe(1);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundError))]
    public async Task GetMeNotFoundId()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;
        await using var context = new DataBaseContext(options);

        var usersService = new UsersService(context);
        await usersService.GetMe(1);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundError))]
    public async Task GetUserNotFoundId()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;
        await using var context = new DataBaseContext(options);

        var usersService = new UsersService(context);
        await usersService.GetUser(1);
    }
}