namespace RollerCoaster.DataTransferObjects.Game.Fetching;

public class LocationDTO
{
    public required int Id { get; set; }
    public required int GameId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string? MapFilePath { get; set; }
    public required int? Width { get; set; }
    public required int? Height { get; set; }
    public required int? BasePlayersXPosition { get; set; }
    public required int? BasePlayersYPosition { get; set; }
}