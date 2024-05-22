namespace RollerCoaster.DataTransferObjects.Session;

public class QuestChangeStatusDTO
{
    public required int SessionId { get; set; }
    public required string Status { get; set; } 
}