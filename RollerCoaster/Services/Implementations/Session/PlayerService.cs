using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Session;
using RollerCoaster.DataTransferObjects.Session.Creation;
using RollerCoaster.DataTransferObjects.Session.Fetching;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Services.Realisations.Session;

public class PlayerService(DataBaseContext dataBaseContext): IPlayerService
{
    public async Task<PlayerDTO> Get(int accessorId, int playerId)
    {
        // TODO: Validation for accessorId
        var player = await dataBaseContext.Players.FindAsync(playerId);
        
        if (player is null)
            throw new NotFoundError("Игрок не найден");

        return new PlayerDTO()
        {
            Attributes = new AttributesDTO(),
            CharacterClassId = player.CharacterClassId,
            CurrentLocationId = player.CurrentLocationId,
            CurrentPosition = player.CurrentPosition,
            HealthPoints = player.HealthPoints,
            Level = player.Level,
            Inventory = new InventoryDTO(),
            Id = player.Id,
            Name = player.Name,
            SessionId = player.SessionId,
            UserId = player.UserId
        };
    }

    public async Task<int> Create(int accessorId, PlayerCreationDTO playerCreationDto)
    {
        var session = await dataBaseContext.Sessions.FindAsync(playerCreationDto.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найдена");
        
        var characterClass = await dataBaseContext.CharacterClasses.FindAsync(playerCreationDto.CharacterClassId);
        if (characterClass is null)
            throw new NotFoundError("Класс персонажа на найден");
        
        var player = new Player()
        {
            AttributesId = 1,
            InventoryId = 1,
            CharacterClassId = playerCreationDto.CharacterClassId,
            CurrentPosition = "0x0",
            CurrentLocationId = 0,
            HealthPoints = 100,
            Level = 1,
            Name = playerCreationDto.Name,
            SessionId = playerCreationDto.SessionId
        };
        await dataBaseContext.AddAsync(player);
        await dataBaseContext.SaveChangesAsync();
        return player.Id;
    }
    
}