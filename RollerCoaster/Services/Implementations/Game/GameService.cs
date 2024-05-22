using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase;
using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.DataTransferObjects.Game.Fetching;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Services.Realisations.Game;

public class GameService(DataBaseContext dataBaseContext) : IGameService
{
    public async Task<GameDTO> Get(int id)
    {
        var game = await dataBaseContext.Games
            .Include(g => g.Items)
            .Include(g => g.Classes)
            .Include(g => g.Locations)
            .Include(g => g.Quests)
            .Include(g => g.Skills)
            .Include(g => g.NonPlayableCharacters)
            .FirstOrDefaultAsync(g => g.Id == id);
        
        if (game is null)
            throw new NotFoundError("Игра не найдена.");

        return new GameDTO
        {
            Id = game.Id,
            Name = game.Name,
            Description = game.Description,
            CreatorId = game.CreatorId,
            
            Locations = game.Locations.Select(l => new LocationDTO
            {
                Id = l.Id,
                GameId = l.GameId,
                Name = l.Name,
                Description = l.Description,
                MapFilePath = l.MapFilePath,
                Sizes = l.Sizes
            }).ToList(),
            
            Items = game.Items.Select(i => new ItemDTO
            {
                Id = i.Id,
                GameId = i.GameId,
                Name = i.Name,
                Description = i.Description,
                ItemType = i.ItemType.ToString() // TODO: это норм?
            }).ToList(),
            
            Quests = game.Quests.Select(q => new QuestDTO
            {
                Id = q.Id,
                GameId = q.GameId,
                Name = q.Name,
                Description = q.Description,
                HiddenDescription = q.HiddenDescription
            }).ToList(),
            
            Skills = game.Skills.Select(s => new SkillDTO
            {
                Id = s.Id,
                GameId = s.GameId,
                Name = s.Name,
                Description = s.Description,
                AvailableForCharacterClassId = s.AvailableForCharacterClassId
            }).ToList(),
            
            NonPlayableCharacters = game.NonPlayableCharacters.Select(npc => new NonPlayableCharacterDTO
            {
                Id = npc.Id,
                GameId = npc.GameId,
                Name = npc.Name,
                BaseLocationId = npc.BaseLocationId,
                BasePosition = npc.BasePosition,
                AvatarFilePath = npc.AvatarFilePath
            }).ToList(),
            
            Classes = game.Classes.Select(cls => new CharacterClassDTO
            {
                Id = cls.Id,
                GameId = cls.GameId,
                Name = cls.Name,
                Description = cls.Description,
            }).ToList()
        };
    }

    public async Task<int> Create(int accessorUserId, GameCreationDTO gameCreationDto)
    {
        var game = new DataBase.Models.Game.Game
        {
            Description = gameCreationDto.Description,
            Name = gameCreationDto.Name,
            CreatorId = accessorUserId
        };
        await dataBaseContext.Games.AddAsync(game);
        await dataBaseContext.SaveChangesAsync();

        return game.Id;
    }

    public async Task Delete(int accessorUserId, int id)
    {
        var game = await dataBaseContext.Games.FindAsync(id);
        
        if (game is null)
            throw new NotFoundError("Игра не найдена.");

        if (game.CreatorId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");
        
        dataBaseContext.Remove(game);
        await dataBaseContext.SaveChangesAsync();
    }
}