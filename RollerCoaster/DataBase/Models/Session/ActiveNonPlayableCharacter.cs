using System.ComponentModel.DataAnnotations.Schema;

namespace RollerCoaster.DataBase.Models.Session;

public class ActiveNonPlayableCharacter
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required int NonPlayableCharacterId { get; set; } // TODO: можно сделать нав-свойство
    public required int SessionId { get; set; }
    public required int HealthPoints { get; set; }
    public required int CurrentXPosition { get; set; }
    public required int CurrentYPosition { get; set; }
}