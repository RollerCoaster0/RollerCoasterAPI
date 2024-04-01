namespace RollerCoaster.DataBase.Models.Game;

public class Game
{
    public int Id { get; set; }
    public required int CreatorId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public List<Location> Locations { get; set; } = [];
    public List<CharacterClass> Classes { get; set; } = [];
    public List<Quest> Quests { get; set; } = [];
    public List<Item> Items { get; set; } = [];
    public List<NonPlayableCharacter> NonPlayableCharacters { get; set; } = [];
    public List<Skill> Skills { get; set; } = [];
}