using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game;

namespace RollerCoaster.Services;

public class GameNotFoundException(string message) : Exception(message)
{
    
}

public class GameService (DataBaseContext dataBaseContext) : IGameService
{
    public async Task<GameDTO?> Get(int id)
    {
        var game = await dataBaseContext.Games.FindAsync(id);
        
        if (game is null)
            return null;

        return new GameDTO()
        {
            Name = game.Name,
            Description = game.Description,
            Locations = game.Locations.Select(l => new LocationDTO()
            {
                Name = l.Name,
                Description = l.Description,
                Id = "abc"
                
            }).ToList(),
            Items = game.Items.Select(i => new  ItemDTO(){
                Name = game.Name,
                Description = i.Description,
                ItemType = "iii"
            
            }).ToList(),
            Quests = game.Quests.Select(quest => new QuestDTO()
            {
                Name = game.Name,
                Description = quest.Description,
                HiddenDescription = "hhh"
            }).ToList(),
            Skills = game.Skills.Select(skill => new SkillDTO()
            {
                Name = game.Name,
                Description = skill.Description,
                ForClassId = "fff"
            }).ToList(),
            NonPlayableCharacters = game.NonPlayableCharacters.Select(npc => new NonPlayableCharacterDTO()
            {
                Name = game.Name,
                BaseLocationId = "bbb"
            }).ToList(),
            Classes = game.Classes.Select(cls => new CharacterClassDTO()
            {
                Name = game.Name,
                Description = cls.Description,
                Id = "iii"
            }).ToList(),
           
                
        };

    }

    public async Task<int> Create(GameDTO gameDto)
    {
        var game = new Game()
        {
            Classes = gameDto.Classes.Select(cls => new CharacterClass()
            {
                CharacterClassId = 0,
                Description = cls.Description,
                GameId = 0,
                Name = cls.Name,
            }).ToList(),

            Description = gameDto.Description,
            Items = gameDto.Items.Select(i => new Item()
            {
                Description = i.Description,
                GameId = 0,
                Name = i.Name,
                ItemId = 0,
                ItemType = ItemType.Armor,
            }).ToList(),

            Quests = gameDto.Quests.Select(q => new Quest()
            {
                Description = q.Description,
                GameId = 0,
                Name = q.Name,
                HiddenDescription = q.HiddenDescription,
                QuestId = 0
            }).ToList(),

            Locations = gameDto.Locations.Select(l => new Location()
            {
                Description = l.Description,
                GameId = 0,
                LocationId = 0,
                MapFileName = "",
                Name = l.Name
            }).ToList(),

            NonPlayableCharacters = gameDto.NonPlayableCharacters.Select(npc => new NonPlayableCharacter()
            {
                BaseLocationId = 0,
                GameId = 0,
                Name = npc.Name,
                NonPlayableCharacterId = 0
            }).ToList(),

            Skills = gameDto.Skills.Select(s => new Skill()
            {
                AvailableForCharacterClassId = 0,
                Description = s.Description,
                Name = s.Name,
                GameId = 0,
                SkillId = 0
            }).ToList(),

            Name = gameDto.Name

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