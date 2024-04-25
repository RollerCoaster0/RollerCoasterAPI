using System.ComponentModel.DataAnnotations;

namespace RollerCoaster.DataTransferObjects.Game.Creation;

public class NonPlayableCharacterCreationDTO 
{
    public required int GameId { get; set; }
    [StringLength(64)] public required string Name { get; set; }
    public required int BaseLocationId { get; set; }
    public required string BasePosition { get; set; }
}