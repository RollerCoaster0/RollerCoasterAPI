using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game.CharacterClasses;
using RollerCoaster.Services.Implementations.Game;

namespace RollerCoaster.Tests;

[TestClass]
public class CharacterClassServiceTest
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
    public async Task FindCharacterClassTest()
    {
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseSqlite(connection)
            .Options;
        await using var context = new DataBaseContext(options);
        
        await context.Games.AddAsync(_game);
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
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseSqlite(connection)
            .Options;
        await using var context = new DataBaseContext(options);
        
        context.Games.Add(_game);
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
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseSqlite(connection)
            .Options;
        await using var context = new DataBaseContext(options);
        
        context.Games.Add(_game);
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
        await service.Delete(1, 1);
    }
}
