namespace RollerCoaster.DataBase.Models.Session;

public class ActiveNonPlayableCharacter
{
    public int Id { get; set; }
    public required int NonPlayableCharacterId { get; set; }
    public required int SessionId { get; set; }
    public required int HealthPoints { get; set; }
    public required int CurrentXPosition { get; set; }
    public required int CurrentYPosition { get; set; }
    public required int AttributesId { get; set; }
    public required int InventoryId { get; set; }
}