namespace RollerCoaster.DataTransferObjects.Game.Creation;

public class SkillCreationDTO
{
    public required int GameId { get; set; } 
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int? AvailableForCharacterClassId { get; set; }
}