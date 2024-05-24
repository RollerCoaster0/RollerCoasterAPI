using RollerCoaster.DataTransferObjects.Session.Chat.Messages;

namespace RollerCoaster.DataTransferObjects.Session.Chat.Actions;

public class NewMessageAction
{
    public required int SessionId { get; set; }
    public required MessageDTO Message { get; set; }
}