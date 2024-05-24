using RollerCoaster.DataTransferObjects.Session.Chat;

namespace RollerCoaster.DataTransferObjects.LongPoll.Updates;

public class ChatActionUpdate
{
    public required int SessionId { get; set; }
    public required ChatActionDTO ChatAction { get; set; }
}