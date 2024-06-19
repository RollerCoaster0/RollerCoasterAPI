using System.ComponentModel.DataAnnotations.Schema;

namespace RollerCoaster.DataBase.Models.Session.Messages;

public class Message
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required int SessionId { get; set; }
    public required DateTimeOffset Time { get; set; }
    
    public TextMessage? TextMessage { get; set; }
    public required int? TextMessageId { get; set; }
    
    public RollMessage? RollMessage { get; set; }
    public required int? RollMessageId { get; set; }
    
    public UsedSkillMessage? UsedSkillMessage { get; set; }
    public required int? UsedSkillMessageId { get; set; }
}   