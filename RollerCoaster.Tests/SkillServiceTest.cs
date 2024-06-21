using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game.Skills;
using RollerCoaster.Services.Implementations.Game;

namespace RollerCoaster.Tests;

[TestClass]
public class SkillServiceTest
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
        context.Skills.Add(new Skill
        {
            AvailableOnlyForCharacterClassId = 1,
            AvailableOnlyForNonPlayableCharacterId = 1,
            Description = "Привет",
            GameId = 1,
            Name = "куку"
        });
        context.Skills.Add(new Skill
        {
            AvailableOnlyForCharacterClassId = 1,
            AvailableOnlyForNonPlayableCharacterId = 1,
            Description = "Пока",
            GameId = 1,
            Name = "дыды"
        });
        await context.SaveChangesAsync();
        
        var service = new SkillService(context);
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
        
        var service = new SkillService(context);
        await service.Create(1, new SkillCreationDTO
        {
            AvailableOnlyForCharacterClassId = null,
            AvailableOnlyForNonPlayableCharacterId = null,
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
        context.Skills.Add(new Skill
        {
            AvailableOnlyForCharacterClassId = 1,
            AvailableOnlyForNonPlayableCharacterId = 1,
            Description = "Привет",
            GameId = 1,
            Name = "куку"
        });
        context.Skills.Add(new Skill
        {
            AvailableOnlyForCharacterClassId = 1,
            AvailableOnlyForNonPlayableCharacterId = 1,
            Description = "Пока",
            GameId = 1,
            Name = "дыды"
        });
        await context.SaveChangesAsync();
        
        var service = new SkillService(context);
        await service.Delete(1, 1);
    }
}