using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Session;
using RollerCoaster.DataTransferObjects;
using RollerCoaster.DataTransferObjects.Session;
using RollerCoaster.DataTransferObjects.Session.Creation;
using RollerCoaster.DataTransferObjects.Session.Fetching;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Services.Realisations.Session;

public class PlayerService(DataBaseContext dataBaseContext): IPlayerService
{
    public async Task<PlayerDTO> Get(int accessorUserId, int playerId)
    {
        // TODO: Validation for accessorId
        var player = await dataBaseContext.Players.FindAsync(playerId);
        
        if (player is null)
            throw new NotFoundError("Игрок не найден");

        return new PlayerDTO
        {
            Attributes = new AttributesDTO(),
            CharacterClassId = player.CharacterClassId,
            CurrentXPosition = player.CurrentXPosition,
            CurrentYPosition = player.CurrentYPosition,
            HealthPoints = player.HealthPoints,
            Level = player.Level,
            Inventory = new InventoryDTO(),
            Id = player.Id,
            Name = player.Name,
            SessionId = player.SessionId,
            UserId = player.UserId
        };
    }

    public async Task<int> Create(int accessorUserId, PlayerCreationDTO playerCreationDto)
    {
        var session = await dataBaseContext.Sessions.FindAsync(playerCreationDto.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найдена");
        
        var characterClass = await dataBaseContext.CharacterClasses.FindAsync(playerCreationDto.CharacterClassId);
        if (characterClass is null)
            throw new NotFoundError("Класс персонажа на найден");
        
        // TODO: брать начальную позицию из объекта локации
        var player = new Player
        {
            AttributesId = 1,
            InventoryId = 1,
            CharacterClassId = playerCreationDto.CharacterClassId,
            CurrentXPosition = 0,
            CurrentYPosition = 0,
            HealthPoints = 100,
            Level = 1,
            Name = playerCreationDto.Name,
            SessionId = playerCreationDto.SessionId
        };
        await dataBaseContext.AddAsync(player);
        await dataBaseContext.SaveChangesAsync();
        return player.Id;
    }

    public Task Move(int accessorUserId, int playerId, MoveSomeoneDTO moveSomeoneDto)
    {
        throw new NotImplementedException();
    }

    public Task ChangeHealthPoints(int accessorUserId, int playerId, ChangeHealthPointsDTO changeHealthPointsDto)
    {
        throw new NotImplementedException();
    }

    public Task UseSkill(int accessorUserId, int playerId, UseSkillDTO useSkillDto)
    {
        throw new NotImplementedException();
    }

    public Task<RollResultDTO> Roll(int accessorUserId, int playerId, RollDTO rollDto)
    {
        throw new NotImplementedException();
    }
}