namespace RollerCoaster.DataBase.Models.Game;

public class Skill
{
    public required int GameId { get; set; } // key
    public required int SkillId { get; set; } // key
    public required string Name { get; set; }
    public required string Description { get; set; }
    // если не null, то этот скилл только для конкретного класса
    public required int? AvailableForCharacterClassId { get; set; } 
}