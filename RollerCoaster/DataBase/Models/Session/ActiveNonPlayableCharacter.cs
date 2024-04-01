namespace RollerCoaster.DataBase.Models.Session;

public class ActiveNonPlayableCharacter
{
    public int Id { get; set; }
    public required int AttributesId { get; set; }
    public required int InventoryId { get; set; }
    public required int SessionId { get; set; }
}