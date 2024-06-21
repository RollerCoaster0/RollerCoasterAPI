using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Implementations.Game;

namespace RollerCoaster.Tests;

[TestClass]
public class GameServiceTest
{
    private readonly Game _game = new()
    {
        Id = 1,
        BaseLocationId = 1,
        Classes = [],
        CreatorUserId = 1,
        Description = "test",
        Items = [],
        Locations = [],
        Name = "test",
        NonPlayableCharacters = [],
        Quests = [],
        Skills = []
    };
    
    [TestMethod]
    public async Task GetCorrect()
    {
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseSqlite(connection)
            .Options;
        await using var context = new DataBaseContext(options);

        await context.Games.AddAsync(_game);
        await context.SaveChangesAsync();

        var gameService = new GameService(context);
        var game = await gameService.Get(1);
        
        Assert.AreEqual("test", game.Name);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundError))]
    public async Task GetNotFound()
    {
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseSqlite(connection)
            .Options;
        await using var context = new DataBaseContext(options);

        var gameService = new GameService(context);
        await gameService.Get(1);
    }

    [TestMethod]
    public async Task CreateTest()
    {
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseSqlite(connection)
            .Options;
        await using var context = new DataBaseContext(options);
        
        var gameService = new GameService(context);
        var game = gameService.Create(1, new GameCreationDTO
        {
            Description = "test",
            Name = "test"
        });

        var test = await context.Games.FindAsync(game.Id);
        Assert.AreEqual(game.Id, test?.Id);
    }

    [TestMethod]
    public async Task DeleteCorrect()
    {
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseSqlite(connection)
            .Options;
        await using var context = new DataBaseContext(options);
        
        await context.Games.AddAsync(_game);
        await context.SaveChangesAsync();
        
        var gameService = new GameService(context);
        await gameService.Delete(1, 1);
        
        Assert.IsNull(await context.Games.FindAsync(1));
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundError))]
    public async Task DeleteNotFound()
    {
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseSqlite(connection)
            .Options;
        await using var context = new DataBaseContext(options);
        
        var gameService = new GameService(context);
        await gameService.Delete(1, 1);
    }

    [TestMethod]
    [ExpectedException(typeof(AccessDeniedError))]
    public async Task DeleteAccessDenied()
    {
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseSqlite(connection)
            .Options;
        await using var context = new DataBaseContext(options);
        
        await context.Games.AddAsync(_game);
        await context.SaveChangesAsync();
        
        var gameService = new GameService(context);
        await gameService.Delete(0, 1);
    }
}