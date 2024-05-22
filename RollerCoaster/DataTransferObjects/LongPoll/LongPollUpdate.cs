using RollerCoaster.DataTransferObjects.LongPoll.Updates;

namespace RollerCoaster.DataTransferObjects.LongPoll;

/// <summary>
/// This object contains one of described updates 
/// </summary>
public class LongPollUpdate
{
    public required NewMessageUpdate? NewMessageUpdate { get; set; }
    public required PlayerMoveUpdate? PlayerMoveUpdate { get; set; }
    public required RollUpdate? RollUpdate { get; set; }
    public required SkillUsedUpdate? SkillUsedUpdate { get; set; }
    public required QuestStatusUpdate? QuestStatusUpdate { get; set; }
    public required PlayerStatusUpdate? PlayerStatusUpdate { get; set; }
    public required SessionStartedUpdate? SessionStartedUpdate { get; set; }
}