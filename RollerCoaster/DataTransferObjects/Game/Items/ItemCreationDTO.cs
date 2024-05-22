using System.ComponentModel.DataAnnotations;

namespace RollerCoaster.DataTransferObjects.Game.Items;

public class ItemCreationDTO
{
    public required int GameId { get; set; }
    [StringLength(64)] public required string Name { get; set; }
    [StringLength(512)] public required string Description { get; set; }
    public required string ItemType { get; set; } // TODO: сделать так, чтобы автоматически конвертило в енам
}
