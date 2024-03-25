namespace RollerCoaster.Models;

public class Skill
{
    public required int GameId { get; set; } // key
    public required int SkillId { get; set; } // key
    public required int AvailableForCharacterClassId { get; set; } // key
    public required string Name { get; set; }
    public required string Description { get; set; }
}