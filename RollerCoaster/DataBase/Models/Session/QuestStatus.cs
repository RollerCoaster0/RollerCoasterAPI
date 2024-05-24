using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RollerCoaster.DataBase.Models.Session;

[PrimaryKey(nameof(SessionId), nameof(QuestId))]
public class QuestStatus
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int SessionId { get; set; }
    public required int QuestId { get; set; }
    public required string Status { get; set; } // started, finished
}