using RollerCoaster.DataTransferObjects.Session.Players;

namespace RollerCoaster.DataTransferObjects.LongPoll.Updates;

public class SkillUsedUpdate
{
    public required int SessionId { get; set; }
    public required PlayerDTO Player { get; set; }
    public required int SkillId { get; set; }
}