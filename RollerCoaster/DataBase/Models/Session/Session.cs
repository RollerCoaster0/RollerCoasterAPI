namespace RollerCoaster.DataBase.Models.Session;

public class Session
{
    public int Id { get; set; }
    public int GameMasterUserId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int CreatorId { get; set; }
    public required DateTimeOffset CreationDate { get; set; }
    public required List<Player> Players { get; set; }
}