using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.DataTransferObjects.Game.Fetching;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Services.Realisations.Game;


public class LocationService(DataBaseContext dataBaseContext): ILocationService
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
            MapFileUrl = location.MapFileName
        };
    }

    public async Task<int> Create(int accessorId, LocationCreationDTO locationCreationDto)
    {
        var game = await dataBaseContext.Games.FindAsync(locationCreationDto.GameId);
        
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorId != accessorId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");

        var location = new Location
        {
            GameId = locationCreationDto.GameId,
            Name = locationCreationDto.Name,
            Description = locationCreationDto.Description,
            MapFileName = ""
        };

        await dataBaseContext.Locations.AddAsync(location);
        await dataBaseContext.SaveChangesAsync();

        return location.Id;
    }

    public async Task Delete(int accessorId, int id)
    {
        var location = await dataBaseContext.Locations.FindAsync(id);
        if (location is null)
            throw new NotFoundError("Локация не найдена.");
        
        var game = await dataBaseContext.Games.FindAsync(location.GameId);
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorId != accessorId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");

        dataBaseContext.Locations.Remove(location);
        await dataBaseContext.SaveChangesAsync();
    }
}