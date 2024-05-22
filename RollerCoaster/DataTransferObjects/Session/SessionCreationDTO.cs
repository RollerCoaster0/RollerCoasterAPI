using System.ComponentModel.DataAnnotations;

namespace RollerCoaster.DataTransferObjects.Session;

public class SessionCreationDTO
{
    [StringLength(64)] public required string Name { get; set; }
    [StringLength(512)] public required string Description { get; set; }
    public required int GameId { get; set; }
}