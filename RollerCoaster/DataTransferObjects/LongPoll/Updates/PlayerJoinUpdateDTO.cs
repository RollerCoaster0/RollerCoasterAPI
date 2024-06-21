using RollerCoaster.DataTransferObjects.Session.Players;

namespace RollerCoaster.DataTransferObjects.LongPoll.Updates;

public class PlayerJoinUpdateDTO
{
    public required int SessionId { get; set; }
    public required PlayerDTO Player { get; set; }
}