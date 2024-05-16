namespace RollerCoaster.DataTransferObjects.Updates;

public class PlayerMoveUpdate
{
    public required int SessionId { get; set; }
    public required string TargetPosition { get; set; }
    public required int FromPlayerId { get; set; }
}