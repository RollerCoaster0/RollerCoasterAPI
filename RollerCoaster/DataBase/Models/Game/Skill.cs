using System.ComponentModel.DataAnnotations.Schema;

namespace RollerCoaster.DataBase.Models.Game;

public class Skill
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } 
    public required int GameId { get; set; } 
    public required string Name { get; set; }
    public required string Description { get; set; }
    
    public required int? AvailableOnlyForCharacterClassId { get; set; }
    public required int? AvailableOnlyForNonPlayableCharacterId { get; set; }
    
    // TODO: можно сделать нав-свойствами их
    // if both null skill can be used by anyone
}