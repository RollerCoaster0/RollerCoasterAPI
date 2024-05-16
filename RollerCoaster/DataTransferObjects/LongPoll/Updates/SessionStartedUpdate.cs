using RollerCoaster.DataTransferObjects.Session.Fetching;

namespace RollerCoaster.DataTransferObjects.Updates;

public class SessionStartedUpdate
{
    public required int SessionId { get; set; }
    public required SessionDTO Session { get; set; }
}