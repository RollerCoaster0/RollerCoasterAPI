namespace RollerCoaster.DataTransferObjects.Game;

public class SkillDTO
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    // здесь указывается идентификатор класса, если скилл уникален для этого класса
    public required string? ForClassId { get; set; }
}