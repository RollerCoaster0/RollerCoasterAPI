namespace RollerCoaster.DataTransferObjects.Session.ActiveNonPlayableCharacter;

public class ActiveNonPlayableCharacterDTO
{
    public required int Id { get; set; }
    public required int NonPlayableCharacterId { get; set; }
    public required int SessionId { get; set; }
    public required int HealthPoints { get; set; }
    public required int CurrentXPosition { get; set; }
    public required int CurrentYPosition { get; set; }
}