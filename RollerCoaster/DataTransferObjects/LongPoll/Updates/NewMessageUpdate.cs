using RollerCoaster.DataTransferObjects.Session.Fetching;

namespace RollerCoaster.DataTransferObjects.Updates;

public class NewMessageUpdate
{
    public required int SessionId { get; set; }
    public required int MessageId { get; set; }
    public required PlayerDTO Player { get; set; }
    public required string Text { get; set; }
}