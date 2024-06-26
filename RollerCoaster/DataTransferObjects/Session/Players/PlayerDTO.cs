namespace RollerCoaster.DataTransferObjects.Session.Players;

public class PlayerDTO
{
    public required int Id { get; set; }
    public required int UserId { get; set; }
    public required int SessionId { get; set; }
    public required string Name { get; set; }
    public required int HealthPoints { get; set; }
    public required int CurrentXPosition { get; set; }
    public required int CurrentYPosition { get; set; }
    public required int CharacterClassId { get; set; }
    public required string? AvatarFilePath { get; set; }
}