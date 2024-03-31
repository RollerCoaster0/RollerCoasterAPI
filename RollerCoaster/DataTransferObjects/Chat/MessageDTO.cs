namespace RollerCoaster.DataTransferObjects.Chat;

public class MessageDTO
{
    public required int MessageId { get; set; }
    public required int SessionId { get; set; }
    public required int SenderId { get; set; }
    public required string Text { get; set; }
    public required DateTimeOffset Time { get; set; }
}