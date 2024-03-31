namespace RollerCoaster.DataBase.Models.Game;

public class Location
{
    public int Id { get; set; }
    public required int GameId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string MapFileName { get; set; }
}