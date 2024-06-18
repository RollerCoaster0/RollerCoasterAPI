using System.ComponentModel.DataAnnotations.Schema;

namespace RollerCoaster.DataBase.Models.Session;

public class Player
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required int UserId { get; set; }
    public required int SessionId { get; set; }
    public required string Name { get; set; }
    public required int CharacterClassId { get; set; } // TODO: можно сделать нав-свойство
    public required int HealthPoints { get; set; }
    public required int CurrentXPosition { get; set; }
    public required int CurrentYPosition { get; set; }
    public required string? AvatarFilePath { get; set; }
}