using RollerCoaster.DataTransferObjects.Game.Fetching;

namespace RollerCoaster.DataTransferObjects.Updates;

public class QuestStatusUpdate
{
    public required int SessionId { get; set; }
    public required QuestDTO Quest { get; set; }
}