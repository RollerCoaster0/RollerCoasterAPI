using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game;

namespace RollerCoaster.Services;

public class GameNotFoundException(string message) : Exception(message);

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
            throw new GameNotFoundException("Игра не найдена");

        return new GameDTO
        {
            Id = game.Id,
            Name = game.Name,
            Description = game.Description,
            
            Locations = game.Locations.Select(l => new LocationDTO
            {
                Id = l.Id,
                GameId = l.GameId,
                Name = l.Name,
                Description = l.Description,
                MapFileUrl = ""
            }).ToList(),
            
            Items = game.Items.Select(i => new ItemDTO
            {
                Id = i.Id,
                GameId = i.GameId,
                Name = i.Name,
                Description = i.Description,
                ItemType = ""
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
                AvailableForCharacterClassId = "fff"
            }).ToList(),
            
            NonPlayableCharacters = game.NonPlayableCharacters.Select(npc => new NonPlayableCharacterDTO
            {
                Id = npc.Id,
                GameId = npc.GameId,
                Name = npc.Name,
                BaseLocationId = npc.BaseLocationId
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

    public async Task<int> Create(GameCreationDTO gameCreationDto)
    {
        var game = new Game
        {
            Classes = gameCreationDto.Classes.Select(cls => new CharacterClass
            {
                Description = cls.Description,
                GameId = 0,
                Name = cls.Name,
            }).ToList(),

            Description = gameCreationDto.Description,
            Items = gameCreationDto.Items.Select(i => new Item
            {
                Description = i.Description,
                GameId = 0,
                Name = i.Name,
                ItemType = ItemType.Armor,
            }).ToList(),

            Quests = gameCreationDto.Quests.Select(q => new Quest
            {
                Description = q.Description,
                GameId = 0,
                Name = q.Name,
                HiddenDescription = q.HiddenDescription,
            }).ToList(),

            Locations = gameCreationDto.Locations.Select(l => new Location
            {
                Description = l.Description,
                GameId = 0,
                MapFileName = "",
                Name = l.Name
            }).ToList(),

            NonPlayableCharacters = gameCreationDto.NonPlayableCharacters.Select(npc => new NonPlayableCharacter
            {
                BaseLocationId = 0,
                GameId = 0,
                Name = npc.Name,
            }).ToList(),

            Skills = gameCreationDto.Skills.Select(s => new Skill
            {
                AvailableForCharacterClassId = 0,
                Description = s.Description,
                Name = s.Name,
                GameId = 0
            }).ToList(),

            Name = gameCreationDto.Name
        };
        await dataBaseContext.Games.AddAsync(game);
        await dataBaseContext.SaveChangesAsync();

        return game.Id;
    }

    public async Task Delete(int id)
    {
        var game = await dataBaseContext.Games.FindAsync(id);
        if (game is null)
            throw new GameNotFoundException("игра не найдена");
        
        dataBaseContext.Remove(game);
        await dataBaseContext.SaveChangesAsync();
    }
}