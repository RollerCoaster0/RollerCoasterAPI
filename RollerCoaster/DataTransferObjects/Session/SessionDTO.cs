namespace RollerCoaster.DataTransferObjects.Session;

public class SessionDTO
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int GameMasterUserId { get; set; }
    public required int GameId { get; set; }
    public required int CurrentPlayersLocationId { get; set; }
    public required bool IsActive { get; set; }
}