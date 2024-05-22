using RollerCoaster.DataTransferObjects.Game.Quests;

namespace RollerCoaster.DataTransferObjects.LongPoll.Updates;

public class QuestStatusUpdate
{
    public required int SessionId { get; set; }
    public required QuestDTO Quest { get; set; }
}