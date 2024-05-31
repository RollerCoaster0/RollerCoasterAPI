using System.ComponentModel.DataAnnotations.Schema;

namespace RollerCoaster.DataBase.Models.Session.Messages;

public class RollMessage
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required int SessionId { get; set; }
    public required int? SenderPlayerId { get; set; }
    public Player? SenderPlayer { get; set; }
    public required int? SenderANPCId { get; set; }
    public ActiveNonPlayableCharacter? SenderANPC { get; set; }
    public required int Result { get; set; } 
    public required int Die { get; set; }
    public required DateTimeOffset Time { get; set; }
}