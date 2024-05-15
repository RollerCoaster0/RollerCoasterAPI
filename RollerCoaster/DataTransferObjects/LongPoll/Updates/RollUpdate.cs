using RollerCoaster.DataTransferObjects.Session.Fetching;

namespace RollerCoaster.DataTransferObjects.Updates;

public class RollUpdate
{
    public required int SessionId { get; set; }
    public required PlayerDTO Player { get; set; }
    public required string Result { get; set; }
}