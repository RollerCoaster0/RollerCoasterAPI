using System.ComponentModel.DataAnnotations;

namespace RollerCoaster.DataTransferObjects.Game.NonPlayableCharacters;

public class NonPlayableCharacterCreationDTO 
{
    public required int GameId { get; set; }
    [StringLength(64)] public required string Name { get; set; }
    public required int BaseLocationId { get; set; }
    public required int BaseXPosition { get; set; }
    public required int BaseYPosition { get; set; }
}