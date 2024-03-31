namespace RollerCoaster.DataBase.Models.Game;

public enum ItemType
{
    Weapon, Armor, Other
}

public class Item
{
    public int Id { get; set; }
    public required int GameId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required ItemType ItemType { get; set; }
}