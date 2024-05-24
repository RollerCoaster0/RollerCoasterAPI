using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RollerCoaster.DataBase.Models.Session;

[PrimaryKey(nameof(Id), nameof(ItemId))]
public class InventoryItem
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required int ItemId { get; set; }
    public required int Count { get; set; }
}
