namespace RollerCoaster.DataTransferObjects.Game.Creation;

public class QuestCreationDTO
{
    public required int GameId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string HiddenDescription { get; set; }
}