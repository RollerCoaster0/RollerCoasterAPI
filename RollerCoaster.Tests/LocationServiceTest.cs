using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Minio;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game.Locations;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Implementations;
using RollerCoaster.Services.Implementations.Common;
using RollerCoaster.Services.Implementations.Game;

namespace RollerCoaster.Tests;
[TestClass]
public class LocationServiceTest
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
        context.Locations.Add(new Location()
        {
            MapFilePath = null,
            Description = "Привет",
            GameId = 1,
            Name = "куку",
            Width = 1,
            Height = 1,
            BasePlayersXPosition = 1,
            BasePlayersYPosition = 1,


        });
        context.Locations.Add(new Location()
        {
            MapFilePath = null,
            Description = "Пока",
            GameId = 1,
            Name = "дыды",
            Width = 1,
            Height = 1,
            BasePlayersXPosition = 1,
            BasePlayersYPosition = 1,
        });
        await context.SaveChangesAsync();
        IMinioClient minio = new MinioClient()
            .WithEndpoint("test")
            .WithCredentials("test", "test")
            .WithSSL(false)
            .Build();
        
        var service = new LocationService(context, new FileTypeValidator(), minio, new ImageCellPainter());
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
        
        IMinioClient minio = new MinioClient()
            .WithEndpoint("test")
            .WithCredentials("test", "test")
            .WithSSL(false)
            .Build();
        
        var service = new LocationService(context, new FileTypeValidator(), minio, new ImageCellPainter());
        await service.Create(1, new LocationCreationDTO()
        {
            IsBase = 1,
            Description = "eeeee",
            GameId = 1,
            Name = "ttt",
        });
        var obj = await service.Get(1);
        Assert.AreEqual("eeeee", obj.Description);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundError))]
    public async Task DeleteTest()
    {
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseSqlite(connection)
            .Options;
        await using var context = new DataBaseContext(options);
        await context.Games.AddAsync(_game);
       
        IMinioClient minio = new MinioClient()
            .WithEndpoint("test")
            .WithCredentials("test", "test")
            .WithSSL(false)
            .Build();
        
        var service = new LocationService(context, new FileTypeValidator(), minio, new ImageCellPainter());
        await service.Create(1, new LocationCreationDTO()
        {
            Description = "Привет",
            Name = "куку",
            IsBase = 0,
            GameId = 1
        });
        context.Locations.Add(new Location
        {
            Description = "Привет",
            GameId = 1,
            Name = "куку",
            MapFilePath = null,
            Width = null,
            Height = null,
            BasePlayersXPosition = null,
            BasePlayersYPosition = null
        });
        await context.SaveChangesAsync();
        await service.Delete(1, 1);
        await service.Get(1);
    }
}
    