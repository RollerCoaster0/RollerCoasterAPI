using RollerCoaster.DataTransferObjects.Game.Skills;
using RollerCoaster.DataTransferObjects.Session.ActiveNonPlayableCharacter;
using RollerCoaster.DataTransferObjects.Session.Players;

namespace RollerCoaster.DataTransferObjects.Session.Chat.Actions;

public class SkillUsedAction

{
    public required int SessionId { get; set; }
    public required ActiveNonPlayableCharacterDTO? ActiveNonPlayableCharacter { get; set; }
    public required PlayerDTO? Player { get; set; }
    public required SkillDTO Skill { get; set; }
}