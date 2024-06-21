using RollerCoaster.DataTransferObjects.Session.ActiveNonPlayableCharacter;
using RollerCoaster.DataTransferObjects.Session.Players;

namespace RollerCoaster.DataTransferObjects.LongPoll.Updates;

public class ChangeHealthPointsUpdateDTO
{
    public required int SessionId { get; set; }
    public required PlayerDTO? Player { get; set; }
    public required ActiveNonPlayableCharacterDTO? ANPC { get; set; }
    public required int NewHP { get; set; }
}