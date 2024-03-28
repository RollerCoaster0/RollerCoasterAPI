namespace RollerCoaster.DataBase.Models.Session;

// TODO: доделать
public class Inventory
{
    public required int GameId { get; set; } // key
    public required int EntityId { get; set; } // key, 
    public required int ItemId { get; set; } // key, local to game
    public required int Count { get; set; }
}


public class Inventory1
{
   //  public required int Inventory1 { get; set; }
    public required int GameId { get; set; } // key
    public required int EntityId { get; set; } // key, 
    public required int ItemId { get; set; } // key, local to game
    public required int Count { get; set; }
}