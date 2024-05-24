using System.ComponentModel.DataAnnotations.Schema;

namespace RollerCoaster.DataBase.Models.Session;

public class UsedRollAction
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required int SessionId { get; set; }
    public required int? PlayerId { get; set; }
    public required int? ANPCId { get; set; }
    public required int Result { get; set; } 
    public required int Die { get; set; }
    public required DateTimeOffset Time { get; set; }
}