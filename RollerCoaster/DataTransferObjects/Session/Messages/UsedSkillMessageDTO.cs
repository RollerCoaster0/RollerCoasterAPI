using RollerCoaster.DataTransferObjects.Game.Skills;
using RollerCoaster.DataTransferObjects.Session.ActiveNonPlayableCharacter;
using RollerCoaster.DataTransferObjects.Session.Players;

namespace RollerCoaster.DataTransferObjects.Session.Messages;

public class UsedSkillMessageDTO
{
    public required PlayerDTO? Player { get; set; }
    public required ActiveNonPlayableCharacterDTO? ANPC { get; set; }
    public required SkillDTO Skill { get; set; }
}