namespace RollerCoaster.DataBase.Models.Game;

public class NonPlayableCharacter
{
    public required int GameId { get; set; }
    public required int NonPlayableCharacterId { get; set; } // local to GameId
    public required int BaseLocationId { get; set; }
    public required string Name { get; set; }
}