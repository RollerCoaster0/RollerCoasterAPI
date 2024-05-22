using System.ComponentModel.DataAnnotations;

namespace RollerCoaster.DataTransferObjects.Game.Creation;

public class LocationCreationDTO
{
    public required int GameId { get; set; }
    [StringLength(64)] public required string Name { get; set; }
    [StringLength(512)] public required string Description { get; set; }
    public required int IsBase { get; set; } = 0; // является ли стартовой локацией (1 или 0)
}