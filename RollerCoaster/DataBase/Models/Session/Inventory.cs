namespace RollerCoaster.DataBase.Models.Session;

// TODO: доделать
public class Inventory
{
    public int Id { get; set; }
    public required int ItemId { get; set; } // key, local to game
    public required int Count { get; set; }
}
