namespace RollerCoaster.Models;

// не забыть сделать скиллы
public class CharacterClass 
{ 
    public required int GameId { get; set; }
    public required int CharacterClassId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}