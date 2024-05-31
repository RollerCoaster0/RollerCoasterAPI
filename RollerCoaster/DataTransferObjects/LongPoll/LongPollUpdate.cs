using RollerCoaster.DataTransferObjects.LongPoll.Updates;
using RollerCoaster.DataTransferObjects.Session;
using RollerCoaster.DataTransferObjects.Session.Messages;

namespace RollerCoaster.DataTransferObjects.LongPoll;

/// <summary>
/// This object contains one of described updates 
/// </summary>
public class LongPollUpdate
{
    public required MessageDTO? NewMessage { get; set; }
    public required QuestStatusUpdateDTO? QuestStatusUpdate { get; set; }
    public required SessionDTO? SessionStarted { get; set; }
    public required MoveUpdateDTO? Move { get; set; }
}