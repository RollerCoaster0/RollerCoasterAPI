using System.ComponentModel.DataAnnotations.Schema;

namespace RollerCoaster.DataBase.Models.Session;

public class Session
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int GameMasterUserId { get; set; }
    public required int GameId { get; set; }
    public required int CurrentPlayersLocationId { get; set; }
    public required bool IsActive { get; set; }
    public List<QuestStatus> QuestStatuses { get; set; } = [];
    public List<Player> Players { get; set; } = [];
    public List<ActiveNonPlayableCharacter> NonPlayableCharacters { get; set; } = [];
}