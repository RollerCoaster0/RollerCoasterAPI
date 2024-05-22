using Microsoft.EntityFrameworkCore;

namespace RollerCoaster.DataBase.Models.Session;

[PrimaryKey(nameof(SessionId), nameof(QuestId))]
public class QuestStatus
{
    public required int SessionId { get; set; }
    public required int QuestId { get; set; }
    public required string Status { get; set; } // started, finished
}