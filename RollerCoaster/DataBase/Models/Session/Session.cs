namespace RollerCoaster.DataBase.Models.Session;

public class Session
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int GameMasterId { get; set; }
    public required int GameId { get; set; }
    public required List<Player> Players { get; set; }
    public required bool IsActive { get; set; }
}