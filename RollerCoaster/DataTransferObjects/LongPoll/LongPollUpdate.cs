using RollerCoaster.DataTransferObjects.LongPoll.Updates;

namespace RollerCoaster.DataTransferObjects.LongPoll;

/// <summary>
/// This object contains one of described updates 
/// </summary>
public class LongPollUpdate
{
    public required ChatActionUpdate? ChatAction { get; set; }
    public required QuestStatusUpdate? QuestStatus { get; set; }
    public required SessionStartedUpdate? SessionStarted { get; set; }
    public required MoveUpdate? Move { get; set; }
}