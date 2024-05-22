using System.ComponentModel.DataAnnotations;

namespace RollerCoaster.DataTransferObjects.Game;

public class GameCreationDTO
{
    [StringLength(64)] public required string Name { get; set; }
    [StringLength(512)] public required string Description { get; set; }
}