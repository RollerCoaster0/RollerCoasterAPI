using RollerCoaster.DataTransferObjects.Session.Players;

namespace RollerCoaster.DataTransferObjects.LongPoll.Updates;

public class RollUpdate
{
    public required int SessionId { get; set; }
    public required PlayerDTO Player { get; set; }
    public required string Result { get; set; }
}