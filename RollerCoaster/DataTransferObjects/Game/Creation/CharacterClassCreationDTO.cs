using System.ComponentModel.DataAnnotations;

namespace RollerCoaster.DataTransferObjects.Game.Creation;

public class CharacterClassCreationDTO 
{
    public required int GameId { get; set; }
    [StringLength(64)] public required string Name { get; set; }
    [StringLength(512)] public required string Description { get; set; }
}