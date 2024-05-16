using RollerCoaster.DataTransferObjects.Session.Fetching;

namespace RollerCoaster.DataTransferObjects.Updates;

public class SkillUsedUpdate
{
    public required int SessionId { get; set; }
    public required PlayerDTO Player { get; set; }
    public required int SkillId { get; set; }
}