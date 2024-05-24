using RollerCoaster.DataTransferObjects.Session.ActiveNonPlayableCharacter;
using RollerCoaster.DataTransferObjects.Session.Common;
using RollerCoaster.DataTransferObjects.Session.Players;

namespace RollerCoaster.DataTransferObjects.Session.Chat.Actions;

public class RollAction
{
    public required int SessionId { get; set; }
    public required PlayerDTO? Player { get; set; }
    public required ActiveNonPlayableCharacterDTO? ActiveNonPlayableCharacter { get; set; }
    public required RollResultDTO Result { get; set; }
}