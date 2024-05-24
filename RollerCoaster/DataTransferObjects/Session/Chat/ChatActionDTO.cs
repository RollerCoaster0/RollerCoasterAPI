using RollerCoaster.DataTransferObjects.LongPoll.Updates;
using RollerCoaster.DataTransferObjects.Session.Chat.Actions;

namespace RollerCoaster.DataTransferObjects.Session.Chat;

public class ChatActionDTO
{
    public required RollAction? Roll { get; set; }
    public required NewMessageAction? NewMessage { get; set; }
    public required SkillUsedAction? SkillUsed { get; set; }
}