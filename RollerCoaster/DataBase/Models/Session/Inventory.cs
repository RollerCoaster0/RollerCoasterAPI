using Microsoft.EntityFrameworkCore;

namespace RollerCoaster.DataBase.Models.Session;

[PrimaryKey(nameof(Id), nameof(ItemId))]
public class Inventory
{
    public int Id { get; set; }
    public required int ItemId { get; set; }
    public required int Count { get; set; }
}
