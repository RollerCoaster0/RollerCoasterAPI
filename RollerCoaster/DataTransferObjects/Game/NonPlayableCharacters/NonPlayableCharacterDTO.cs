namespace RollerCoaster.DataTransferObjects.Game.NonPlayableCharacters;

public class NonPlayableCharacterDTO 
{
    public required int Id { get; set; }
    public required int GameId { get; set; }
    public required string Name { get; set; }
    public required int BaseLocationId { get; set; }
    public required int BaseXPosition { get; set; }
    public required int BaseYPosition { get; set; }
    public required string? AvatarFilePath { get; set; }
}