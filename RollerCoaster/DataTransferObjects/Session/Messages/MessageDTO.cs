namespace RollerCoaster.DataTransferObjects.Session.Messages;

public class MessageDTO
{
    public required int Id { get; set; }
    public required int SessionId { get; set; }
    
    // only one message type provided
    public required RollMessageDTO? RollMessage { get; set; }
    public required TextMessageDTO? TextMessage { get; set; }
    public required UsedSkillMessageDTO? UsedSkillMessage { get; set; }
}