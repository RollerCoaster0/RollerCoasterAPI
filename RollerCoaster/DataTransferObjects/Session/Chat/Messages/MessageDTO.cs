using RollerCoaster.DataTransferObjects.Session.Players;

namespace RollerCoaster.DataTransferObjects.Session.Chat.Messages;

public class MessageDTO
{
    public required int MessageId { get; set; }
    public required int SessionId { get; set; }
    public required PlayerDTO Player { get; set; }
    public required string Text { get; set; }
    public required DateTimeOffset Time { get; set; }
}