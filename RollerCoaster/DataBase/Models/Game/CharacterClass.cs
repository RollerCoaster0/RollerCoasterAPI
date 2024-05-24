using System.ComponentModel.DataAnnotations.Schema;

namespace RollerCoaster.DataBase.Models.Game;

public class CharacterClass 
{ 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required int GameId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}