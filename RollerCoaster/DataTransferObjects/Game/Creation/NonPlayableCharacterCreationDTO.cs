namespace RollerCoaster.DataTransferObjects.Game.Creation;

public class NonPlayableCharacterCreationDTO 
{
    public required int GameId { get; set; }
    public required string Name { get; set; }
    public required int BaseLocationId { get; set; }
}