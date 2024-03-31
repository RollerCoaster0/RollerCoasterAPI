namespace RollerCoaster.DataBase.Models.Game;

public class Skill
{
    public int Id { get; set; } 
    public required int GameId { get; set; } 
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int? AvailableForCharacterClassId { get; set; } 
}