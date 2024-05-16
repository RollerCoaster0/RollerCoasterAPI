using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.Services.Realisations.Game;

namespace RollerCoaster.Tests;

[TestClass]
public class NonPlayableCharacterServiceTest()
{ 
    [TestMethod]
    public void FindCharacterClassTest()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase", new InMemoryDatabaseRoot())
            .Options;
        
        using var context = new DataBaseContext(options);
        context.NonPlayableCharacters.Add(new NonPlayableCharacter()
        {
            BaseLocationId = 1,
            GameId = 1,
            Name = "куку"
        });
        context.NonPlayableCharacters.Add(new NonPlayableCharacter()
        {
            BaseLocationId = 1,
            GameId = 1,
            Name = "дыды"
        });
        context.SaveChanges();
        var service = new NonPlayableCharacterService(context);
        var obj = service.Get(2).Result;
        Assert.AreEqual(obj.Name, "дыды");
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
        var service = new NonPlayableCharacterService(context);
        _ = service.Create(1, new NonPlayableCharacterCreationDTO()
        {
            BaseLocationId = 1,
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
        context.NonPlayableCharacters.Add(new NonPlayableCharacter()
        {
            BaseLocationId = 1,
            GameId = 1,
            Name = "куку"
        });
        context.NonPlayableCharacters.Add(new NonPlayableCharacter()
        {
            BaseLocationId = 1,
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
        var service = new NonPlayableCharacterService(context);
        await service.Delete(1, 1);
    }
}