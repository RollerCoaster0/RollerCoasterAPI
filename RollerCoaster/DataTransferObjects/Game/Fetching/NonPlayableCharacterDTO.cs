namespace RollerCoaster.DataTransferObjects.Game.Fetching;

public class NonPlayableCharacterDTO 
{
    public required int Id { get; set; }
    public required int GameId { get; set; }
    public required int BaseLocationId { get; set; }
    public required string Name { get; set; }
}