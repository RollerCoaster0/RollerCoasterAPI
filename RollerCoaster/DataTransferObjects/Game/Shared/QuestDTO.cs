namespace RollerCoaster.DataTransferObjects.Game;

public class QuestDTO
{
    public required int Id { get; set; }
    public required int GameId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string HiddenDescription { get; set; }
}