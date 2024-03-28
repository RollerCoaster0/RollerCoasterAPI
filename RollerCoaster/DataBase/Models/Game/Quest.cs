namespace RollerCoaster.DataBase.Models.Game;

public class Quest
{
    public required int GameId { get; set; }
    public required int QuestId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string HiddenDescription { get; set; }
}