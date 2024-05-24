using RollerCoaster.DataTransferObjects.Session.ActiveNonPlayableCharacter;
using RollerCoaster.DataTransferObjects.Session.Players;

namespace RollerCoaster.DataTransferObjects.LongPoll.Updates;

public class MoveUpdate
{
    public required int SessionId { get; set; }
    public required PlayerDTO? Player { get; set; }
    public required ActiveNonPlayableCharacterDTO? ActiveNonPlayableCharacter { get; set; }
    public required int X { get; set; }
    public required int Y { get; set; }
}