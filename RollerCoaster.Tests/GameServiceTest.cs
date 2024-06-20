using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Implementations.Game;

namespace RollerCoaster.Tests;

[TestClass]
public class GameServiceTest
{
    [TestMethod]
    public async Task GetCorrect()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;
        await using var context = new DataBaseContext(options);

        await context.Games.AddAsync(new Game
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
        });
        await context.SaveChangesAsync();

        var gameService = new GameService(context);
        var game = await gameService.Get(1);
        
        Assert.AreEqual("test", game.Name);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundError))]
    public async Task GetNotFound()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;
        await using var context = new DataBaseContext(options);

        var gameService = new GameService(context);
        await gameService.Get(1);
    }

    [TestMethod]
    public async Task CreateTest()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;
        await using var context = new DataBaseContext(options);
        
        var gameService = new GameService(context);
        var game = gameService.Create(1, new GameCreationDTO
        {
            Description = "test",
            Name = "test"
        });

        var test = await context.Games.FindAsync(game.Id);
        Assert.AreEqual(game.Id, test.Id);
    }

    [TestMethod]
    public async Task DeleteCorrect()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;
        await using var context = new DataBaseContext(options);
        
        await context.Games.AddAsync(new Game
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
        });
        await context.SaveChangesAsync();
        
        var gameService = new GameService(context);
        await gameService.Delete(1, 1);
        
        Assert.IsNull(await context.Games.FindAsync(1));
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundError))]
    public async Task DeleteNotFound()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;
        await using var context = new DataBaseContext(options);
        
        var gameService = new GameService(context);
        await gameService.Delete(1, 1);
    }

    [TestMethod]
    [ExpectedException(typeof(AccessDeniedError))]
    public async Task DeleteAccessDenied()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;
        await using var context = new DataBaseContext(options);
        
        await context.Games.AddAsync(new Game
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
        });
        await context.SaveChangesAsync();
        
        var gameService = new GameService(context);
        await gameService.Delete(0, 1);
    }
}