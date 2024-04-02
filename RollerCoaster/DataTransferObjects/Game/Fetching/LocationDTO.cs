namespace RollerCoaster.DataTransferObjects.Game.Fetching;

public class LocationDTO
{
    public required int Id { get; set; }
    public required int GameId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string? MapFilePath { get; set; }
}