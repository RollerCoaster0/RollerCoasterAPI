namespace RollerCoaster.DataBase.Models.Session;

public class Player
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required int SessionId { get; set; }
    public required string Name { get; set; }
    public required int Level { get; set; }
    public required int HealthPoints { get; set; }
    public required int CurrentXPosition { get; set; }
    public required int CurrentYPosition { get; set; }
    public required int CharacterClassId { get; set; }
    public required int AttributesId { get; set; }
    public required int InventoryId { get; set; }
}