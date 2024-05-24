using System.ComponentModel.DataAnnotations.Schema;

namespace RollerCoaster.DataBase.Models.Session;

public class UsedSkillAction
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required int SessionId { get; set; }
    public required int? PlayerId { get; set; }
    public required int? ANPCId { get; set; }
    public required int SkillId { get; set; }
    public required DateTimeOffset Time { get; set; }
}