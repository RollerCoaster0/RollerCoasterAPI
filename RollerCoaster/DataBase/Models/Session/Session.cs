using System.ComponentModel.DataAnnotations.Schema;
using RollerCoaster.DataBase.Models.Session.Messages;

namespace RollerCoaster.DataBase.Models.Session;

public class Session
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int GameMasterUserId { get; set; } // TODO: можно сделать нав-свойство
    public required int GameId { get; set; }
    public required int CurrentPlayersLocationId { get; set; } // TODO: можно сделать нав-свойство
    public required bool IsActive { get; set; }
    
    public List<Player> Players { get; set; } = [];
    public List<ActiveNonPlayableCharacter> ActiveNonPlayableCharacters { get; set; } = [];
    public List<QuestStatus> QuestStatuses { get; set; } = [];
    public List<Message> Messages { get; set; } = [];
}