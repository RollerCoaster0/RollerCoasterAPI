using System.ComponentModel.DataAnnotations;

namespace RollerCoaster.DataTransferObjects.Session.Creation;

public class PlayerCreationDTO
{
    public required int CharacterClassId { get; set; }
    [StringLength(64)] public required string Name { get; set; }
    public required int SessionId { get; set; } 
}