using RollerCoaster.DataTransferObjects.Session.Players;

namespace RollerCoaster.DataTransferObjects.Session.Messages;

public class TextMessageDTO
{
    public required PlayerDTO SenderPlayer { get; set; }
    public required string Text { get; set; }
}