using System.ComponentModel.DataAnnotations.Schema;

namespace RollerCoaster.DataBase.Models.Game;

public class NonPlayableCharacter
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } 
    public required int GameId { get; set; }
    public required string Name { get; set; }
    public required int BaseLocationId { get; set; } // TODO: можно сделать нав-свойстом, но не надо 
    public required int BaseXPosition { get; set; }
    public required int BaseYPosition { get; set; }
    public required string? AvatarFilePath { get; set; }
}