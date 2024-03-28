using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game;

namespace RollerCoaster.Services;

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

    public Task<int> Create(GameDTO game)
    {
        throw new NotImplementedException();
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }
}