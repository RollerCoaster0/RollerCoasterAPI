using System.ComponentModel.DataAnnotations.Schema;
using RollerCoaster.DataBase.Models.Game;

namespace RollerCoaster.DataBase.Models.Session.Messages;

public class UsedSkillMessage
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required int SessionId { get; set; }
    public required int? SenderPlayerId { get; set; }
    public Player? SenderPlayer { get; set; }
    public required int? SenderANPCId { get; set; }
    public ActiveNonPlayableCharacter? SenderANPC { get; set; }
    public required int SkillId { get; set; }
    public Skill Skill { get; set; } = null!;
    public required DateTimeOffset Time { get; set; }
}