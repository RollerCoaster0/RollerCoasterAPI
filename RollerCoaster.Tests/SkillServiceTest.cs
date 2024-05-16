using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.Services.Realisations.Game;

namespace RollerCoaster.Tests;

[TestClass]
public class SkillServiceTest()
{ 
    [TestMethod]
    public void FindCharacterClassTest()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;
        
        using var context = new DataBaseContext(options);
        context.Skills.Add(new Skill()
        {
            AvailableForCharacterClassId = 1,
            Description = "Привет",
            GameId = 1,
            Name = "куку"
        });
        context.Skills.Add(new Skill()
        {
            AvailableForCharacterClassId = 1,
            Description = "Пока",
            GameId = 1,
            Name = "дыды"
        });
        context.SaveChanges();
        var service = new SkillService(context);
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
            Skills = []
        });
        context.SaveChanges();
        var service = new SkillService(context);
        _ = service.Create(1, new SkillCreationDTO()
        {
            AvailableForCharacterClassId = 1,
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
        context.Skills.Add(new Skill()
        {
            AvailableForCharacterClassId = 1,
            Description = "Привет",
            GameId = 1,
            Name = "куку"
        });
        context.Skills.Add(new Skill()
        {
            AvailableForCharacterClassId = 1,
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
            Skills = []
        });
        await context.SaveChangesAsync();
        var service = new SkillService(context);
        await service.Delete(1, 1);
    }
}