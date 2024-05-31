using RollerCoaster.DataTransferObjects.Session.ActiveNonPlayableCharacter;
using RollerCoaster.DataTransferObjects.Session.Common;
using RollerCoaster.DataTransferObjects.Session.Players;

namespace RollerCoaster.DataTransferObjects.Session.Messages;

public class RollMessageDTO
{
    public required PlayerDTO? SenderPlayer { get; set; }
    public required ActiveNonPlayableCharacterDTO? SenderANPC { get; set; }
    public required RollResultDTO Result { get; set; }
}