using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game.CharacterClasses;
using RollerCoaster.Services.Implementations.Game;

namespace RollerCoaster.Tests;

[TestClass]
public class CharacterClassServiceTest
{ 
    [TestMethod]
    public async Task FindCharacterClassTest()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;
        await using var context = new DataBaseContext(options);
        
        context.CharacterClasses.Add(new CharacterClass
        {
            Description = "Привет",
            GameId = 1,
            Name = "куку"
        });
        context.CharacterClasses.Add(new CharacterClass
        {
            Description = "Пока",
            GameId = 1,
            Name = "дыды"
        });
        await context.SaveChangesAsync();
        
        var service = new CharacterClassService(context);
        var obj = await service.Get(2);
        
        Assert.AreEqual(obj.Description, "Пока");
    }
    
    [TestMethod]
    public async Task CreateTest()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;
        await using var context = new DataBaseContext(options);
        
        context.Games.Add(new Game
        {
            Classes = [],
            Description = "Ппппп",
            CreatorUserId = 1,
            Items = [],
            Locations = [],
            Name = "ppp",
            NonPlayableCharacters = [],
            Quests = [],
            Skills = [],
            BaseLocationId = 0
        });
        await context.SaveChangesAsync();
        
        var service = new CharacterClassService(context);
        await service.Create(1, new CharacterClassCreationDTO
        {
            Description = "eeeee",
            GameId = 1,
            Name = "ttt"
        });
    }

    [TestMethod]
    public async Task DeleteTest()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;
        await using var context = new DataBaseContext(options);
        
        context.CharacterClasses.Add(new CharacterClass
        {
            Description = "Привет",
            GameId = 1,
            Name = "куку"
        });
        context.CharacterClasses.Add(new CharacterClass
        {
            Description = "Пока",
            GameId = 1,
            Name = "дыды"
        });
        context.Games.Add(new Game
        {
            Classes = [],
            Description = "Ппппп",
            CreatorUserId = 1,
            Items = [],
            Locations = [],
            Name = "ppp",
            NonPlayableCharacters = [],
            Quests = [],
            Skills = [],
            BaseLocationId = 0
        });
        await context.SaveChangesAsync();
        
        var service = new CharacterClassService(context);
        await service.Delete(1, 1);
    }
}
