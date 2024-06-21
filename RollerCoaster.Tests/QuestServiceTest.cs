using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game.Quests;
using RollerCoaster.Services.Implementations.Game;

namespace RollerCoaster.Tests;

[TestClass]
public class QuestServiceTest
{ 
    [TestMethod]
    public async Task FindCharacterClassTest()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;
        await using var context = new DataBaseContext(options);
        
        context.Quests.Add(new Quest
        {
            HiddenDescription = "rrr",
            Description = "Привет",
            GameId = 1,
            Name = "куку"
        });
        context.Quests.Add(new Quest
        {
            HiddenDescription = "gggg",
            Description = "Пока",
            GameId = 1,
            Name = "дыды"
        });
        await context.SaveChangesAsync();
        
        var service = new QuestService(context);
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
        
        var service = new QuestService(context);
        await service.Create(1, new QuestCreationDTO
        {
            HiddenDescription = "ttt",
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
        
        context.Quests.Add(new Quest
        {
            HiddenDescription = "hhhh",
            Description = "Привет",
            GameId = 1,
            Name = "куку"
        });
        context.Quests.Add(new Quest
        {
            HiddenDescription = "yyy",
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
        
        var service = new QuestService(context);
        await service.Delete(1, 1);
    }
}


    
