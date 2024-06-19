using Minio;
using Minio.DataModel.Args;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game.Locations;
using RollerCoaster.Services.Abstractions;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Services.Implementations.Game;

public class LocationService(
    DataBaseContext dataBaseContext,
    IFileTypeValidator fileTypeValidator,
    IMinioClient minioClient,
    IImageCellPainter imageCellPainter
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

    public async Task<LoadedMapDTO> LoadMap(int accessorUserId, int id, LocationMapLoadDTO locationMapLoadDto)
    {
        var location = await dataBaseContext.Locations.FindAsync(id);
        if (location is null)
            throw new NotFoundError("Локация не найдена.");
        
        var game = await dataBaseContext.Games.FindAsync(location.GameId);
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorUserId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");
        
        if (!fileTypeValidator.ValidateImageFileType(locationMapLoadDto.File))
            throw new ProvidedDataIsInvalidError("Формат файла не поддерживается.");
        
        await using var pictureWithCell = await imageCellPainter.DrawCell(
            locationMapLoadDto.File.OpenReadStream(),
            locationMapLoadDto.Width,
            locationMapLoadDto.Height);
        
        string uniqName = Guid.NewGuid().ToString("N");
        const string ext = "png";
        string objectName = $"{uniqName}.{ext}";
        
        await minioClient.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket("images")
                .WithObject(objectName) 
                .WithObjectSize(pictureWithCell.Length)
                .WithStreamData(pictureWithCell));

        location.MapFilePath = $"images/{objectName}";
        location.Height = locationMapLoadDto.Height;
        location.Width = locationMapLoadDto.Width;
        location.BasePlayersXPosition = locationMapLoadDto.BasePlayersXPosition;
        location.BasePlayersYPosition = locationMapLoadDto.BasePlayersYPosition;
        
        await dataBaseContext.SaveChangesAsync();
        
        return new LoadedMapDTO
        {
            MapFilePath = location.MapFilePath
        };
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