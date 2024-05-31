namespace RollerCoaster.DataTransferObjects.Session.Quests;

public class QuestChangeStatusDTO
{
    public required int SessionId { get; set; }
    public required string Status { get; set; } 
}