namespace RollerCoaster.DataTransferObjects.Session.Quests;

public class QuestStatusDTO
{
    public required int SessionId { get; set; }
    public required int QuestId { get; set; }
    public required string Status { get; set; } // started, finished
}