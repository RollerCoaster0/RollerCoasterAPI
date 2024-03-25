namespace RollerCoaster.Models;

// реализовать скиллы для каждого класса и общие
public class Game
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required List<Location> Locations { get; set; }
    public required List<CharacterClass> Classes { get; set; }
    public required List<Quest> Quests { get; set; }
    public required List<Item> Items { get; set; }
    public required List<NonPlayableCharacter> NonPlayableCharacters { get; set; }
}