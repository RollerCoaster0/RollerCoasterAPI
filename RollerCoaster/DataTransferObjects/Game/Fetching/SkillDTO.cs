namespace RollerCoaster.DataTransferObjects.Game;

public class SkillDTO
{
    public required int Id { get; set; } 
    public required int GameId { get; set; } 
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string? AvailableForCharacterClassId { get; set; }
}