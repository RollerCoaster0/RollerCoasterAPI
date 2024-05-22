using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game.Quests;
using RollerCoaster.Services.Implementations.Game;

namespace RollerCoaster.Tests;

[TestClass]
public class QuestServiceTest()
{ 
    [TestMethod]
    public void FindCharacterClassTest()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;
        
        using var context = new DataBaseContext(options);
        context.Quests.Add(new Quest()
        {
            HiddenDescription = "rrr",
            Description = "Привет",
            GameId = 1,
            Name = "куку"
        });
        context.Quests.Add(new Quest()
        {
            HiddenDescription = "gggg",
            Description = "Пока",
            GameId = 1,
            Name = "дыды"
        });
        context.SaveChanges();
        var service = new QuestService(context);
        var obj = service.Get(2).Result;
        Assert.AreEqual(obj.Description, "Пока");
    }
    
    [TestMethod]
    public void CreateTest()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;
        
        using var context = new DataBaseContext(options);
        context.Games.Add(new Game()
        {
            Classes = [],
            Description = "Ппппп",
            CreatorId = 1,
            Items = [],
            Locations = [],
            Name = "ppp",
            NonPlayableCharacters = [],
            Quests = [],
            Skills = [],
            BaseLocationId = 0
        });
        context.SaveChanges();
        var service = new QuestService(context);
        _ = service.Create(1, new QuestCreationDTO()
        {
            HiddenDescription = "ttt",
            Description = "eeeee",
            GameId = 1,
            Name = "ttt"
        }).Result;
    }

    [TestMethod]
    public async Task DeleteTest()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;
        
        await using var context = new DataBaseContext(options);
        context.Quests.Add(new Quest()
        {
            HiddenDescription = "hhhh",
            Description = "Привет",
            GameId = 1,
            Name = "куку"
        });
        context.Quests.Add(new Quest()
        {
            HiddenDescription = "yyy",
            Description = "Пока",
            GameId = 1,
            Name = "дыды"
        });
        context.Games.Add(new Game()
        {
            Classes = [],
            Description = "Ппппп",
            CreatorId = 1,
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


    
