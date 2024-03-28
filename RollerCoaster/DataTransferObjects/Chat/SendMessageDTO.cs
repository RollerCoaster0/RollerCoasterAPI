namespace RollerCoaster.DataTransferObjects.Chat;

public class SendMessageDTO
{
    public required string Text { get; set; }
    public required int SessionId { get; set; }
}