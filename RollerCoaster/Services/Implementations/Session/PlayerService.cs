using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Session;
using RollerCoaster.DataTransferObjects.Game.Skills;
using RollerCoaster.DataTransferObjects.Session.Common;
using RollerCoaster.DataTransferObjects.Session.Players;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Services.Implementations.Session;

public class PlayerService(
    DataBaseContext dataBaseContext,
    IRollService rollService): IPlayerService
{
    public async Task<PlayerDTO> Get(int accessorUserId, int playerId)
    {
        var player = await dataBaseContext.Players.FindAsync(playerId);
        
        if (player is null)
            throw new NotFoundError("Игрок не найден");
        
        var isUserMemberOfSession = await dataBaseContext.Players
            .AnyAsync(p => p.SessionId == player.SessionId && p.UserId == accessorUserId);
        
        var isUserGameMasterOfSession = await dataBaseContext.Sessions
            .AnyAsync(s => s.Id == player.SessionId && s.GameMasterUserId == accessorUserId);
        
        if (!isUserMemberOfSession && !isUserGameMasterOfSession)
            throw new AccessDeniedError("У вас нет доступа к этой сессии.");

        return new PlayerDTO
        {
            CharacterClassId = player.CharacterClassId,
            CurrentXPosition = player.CurrentXPosition,
            CurrentYPosition = player.CurrentYPosition,
            HealthPoints = player.HealthPoints,
            Level = player.Level,
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
            throw new NotFoundError("Сессия не найдена.");
        
        var characterClass = await dataBaseContext.CharacterClasses.FindAsync(playerCreationDto.CharacterClassId);
        if (characterClass is null)
            throw new NotFoundError("Класс персонажа на найден.");
        
        var game = await dataBaseContext.Games.FindAsync(session.GameId);
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        var location = await dataBaseContext.Locations.FindAsync(game.BaseLocationId!.Value);
        if (location is null)
            throw new NotFoundError("Локация не найдена.");
        
        var player = new Player
        {
            UserId = accessorUserId,
            CharacterClassId = playerCreationDto.CharacterClassId,
            CurrentXPosition = location.BasePlayersXPosition!.Value,
            CurrentYPosition = location.BasePlayersYPosition!.Value,
            HealthPoints = 100,
            Level = 1,
            Name = playerCreationDto.Name,
            SessionId = playerCreationDto.SessionId
        };
        await dataBaseContext.AddAsync(player);
        await dataBaseContext.SaveChangesAsync();
        
        return player.Id;
    }

    public async Task Move(int accessorUserId, int playerId, MoveSomeoneDTO moveSomeoneDto)
    {
        var player = await dataBaseContext.Players.FindAsync(playerId);
        if (player is null)
            throw new NotFoundError("Игрок не найден.");

        var session = await dataBaseContext.Sessions.FindAsync(player.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найден.");
        
        if (session.GameMasterUserId != accessorUserId && accessorUserId != player.UserId)
            throw new AccessDeniedError("У вас нет доступа к этому.");
        
        // TODO: validate coordinates 
        player.CurrentXPosition = moveSomeoneDto.X;
        player.CurrentYPosition = moveSomeoneDto.Y;

        await dataBaseContext.SaveChangesAsync();
    }

    public async Task ChangeHealthPoints(int accessorUserId, int playerId, ChangeHealthPointsDTO changeHealthPointsDto)
    {
        var player = await dataBaseContext.Players.FindAsync(playerId);
        if (player is null)
            throw new NotFoundError("Игрок не найден.");

        var session = await dataBaseContext.Sessions.FindAsync(player.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найден.");
        
        if (session.GameMasterUserId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этому.");
        
        player.HealthPoints = changeHealthPointsDto.HP;
        
        await dataBaseContext.SaveChangesAsync();
    }

    public async Task UseSkill(int accessorUserId, int playerId, UseSkillDTO useSkillDto)
    {
        var player = await dataBaseContext.Players.FindAsync(playerId);
        if (player is null)
            throw new NotFoundError("Игрок не найден.");

        var session = await dataBaseContext.Sessions.FindAsync(player.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найден.");
        
        if (session.GameMasterUserId != accessorUserId && accessorUserId != player.UserId)
            throw new AccessDeniedError("У вас нет доступа к этому.");
        
        var skill = await dataBaseContext.Skills.FindAsync(useSkillDto.SkillId);
        if (skill is null)
            throw new NotFoundError("Скилл не найден.");
    }

    public async Task<RollResultDTO> Roll(int accessorUserId, int playerId, RollDTO rollDto)
    {
        var player = await dataBaseContext.Players.FindAsync(playerId);
        if (player is null)
            throw new NotFoundError("Игрок не найден.");

        var session = await dataBaseContext.Sessions.FindAsync(player.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найден.");
        
        if (accessorUserId != player.UserId)
            throw new AccessDeniedError("У вас нет доступа к этому.");
        
        var rollResult = await rollService.Roll(rollDto.Die);
        
        return new RollResultDTO
        {
            Result = rollResult,
            Die = rollDto.Die
        };
    }
}