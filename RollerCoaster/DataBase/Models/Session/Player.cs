namespace RollerCoaster.DataBase.Models.Session;

public class Player
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required int Level { get; set; }
    public required int HealthPoints { get; set; }
    public required string CurrentPosition { get; set; }
    public required int CurrentLocationId { get; set; }
    public required int CharacterClassId { get; set; }
    public required string Name { get; set; }
    public required int AttributesId { get; set; }
    public required int InventoryId { get; set; }
    public required int SessionId { get; set; }
}