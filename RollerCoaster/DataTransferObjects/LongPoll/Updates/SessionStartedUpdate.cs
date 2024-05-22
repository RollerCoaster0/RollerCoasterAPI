using RollerCoaster.DataTransferObjects.Session;

namespace RollerCoaster.DataTransferObjects.LongPoll.Updates;

public class SessionStartedUpdate
{
    public required int SessionId { get; set; }
    public required SessionDTO Session { get; set; }
}