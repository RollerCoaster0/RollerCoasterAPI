namespace RollerCoaster.Models;

public class Location
{
    public required int GameId { get; set; }
    public required int LocationId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string MapFileName { get; set; }
}