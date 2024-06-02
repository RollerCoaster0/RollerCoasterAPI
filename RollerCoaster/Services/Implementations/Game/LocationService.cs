using Minio;
using Minio.DataModel.Args;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game.Locations;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Services.Implementations.Game;

public class LocationService(
    DataBaseContext dataBaseContext,
    IFileTypeValidator fileTypeValidator,
    IMinioClient minioClient
    ): ILocationService
{
    public async Task<LocationDTO> Get(int id)
    {
        var location = await dataBaseContext.Locations.FindAsync(id);

        if (location is null)
            throw new NotFoundError("Локация не найдена.");

        return new LocationDTO
        {
            Id = location.Id,
            GameId = location.GameId,
            Name = location.Name,
            Description = location.Description,
            MapFilePath = location.MapFilePath,
            Height = location.Height,
            Width = location.Width,
            BasePlayersXPosition = location.BasePlayersXPosition,
            BasePlayersYPosition = location.BasePlayersYPosition
        };
    }

    public async Task<int> Create(int accessorUserId, LocationCreationDTO locationCreationDto)
    {
        var game = await dataBaseContext.Games.FindAsync(locationCreationDto.GameId);
        
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorUserId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");

        var location = new Location
        {
            GameId = locationCreationDto.GameId,
            Name = locationCreationDto.Name,
            Description = locationCreationDto.Description,
            MapFilePath = null,
            Height = null,
            Width = null,
            BasePlayersXPosition = null,
            BasePlayersYPosition = null
        };
        

        await dataBaseContext.Locations.AddAsync(location);
        await dataBaseContext.SaveChangesAsync();

        if (locationCreationDto.IsBase == 1)
            game.BaseLocationId = location.Id;
        
        await dataBaseContext.SaveChangesAsync();

        return location.Id;
    }

    public async Task LoadMap(int accessorUserId, LocationMapLoadDTO locationMapLoadDto)
    {
        var location = await dataBaseContext.Locations.FindAsync(locationMapLoadDto.LocationId);
        if (location is null)
            throw new NotFoundError("Локация не найдена.");
        
        var game = await dataBaseContext.Games.FindAsync(location.GameId);
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorUserId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");
        
        if (!fileTypeValidator.ValidateImageFileType(locationMapLoadDto.File))
            throw new ProvidedDataIsInvalidError("Формат файла не поддерживается.");
        
        string uniqName = Guid.NewGuid().ToString("N");
        string ext = locationMapLoadDto.File.FileName.Split(".").Last().ToLower();
        string objectName = $"{uniqName}.{ext}";
        
        await minioClient.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket("images")
                .WithObject(objectName) 
                .WithObjectSize(locationMapLoadDto.File.Length)
                .WithStreamData(locationMapLoadDto.File.OpenReadStream()));

        location.MapFilePath = $"images/{objectName}";
        location.Height = locationMapLoadDto.Height;
        location.Width = locationMapLoadDto.Width;
        location.BasePlayersXPosition = locationMapLoadDto.BasePlayersXPosition;
        location.BasePlayersYPosition = locationMapLoadDto.BasePlayersYPosition;
        
        await dataBaseContext.SaveChangesAsync();
    }

    public async Task Delete(int accessorUserId, int id)
    {
        var location = await dataBaseContext.Locations.FindAsync(id);
        if (location is null)
            throw new NotFoundError("Локация не найдена.");
        
        var game = await dataBaseContext.Games.FindAsync(location.GameId);
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorUserId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");

        dataBaseContext.Locations.Remove(location);
        await dataBaseContext.SaveChangesAsync();
    }
}