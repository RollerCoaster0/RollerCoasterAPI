namespace RollerCoaster.DataTransferObjects.Session.Fetching;

public class SessionDTO
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int GameMasterId { get; set; }
    public required int GameId { get; set; }
    public required List<PlayerDTO> Players { get; set; }
    public required bool IsActive { get; set; }
}