using System.ComponentModel.DataAnnotations.Schema;

namespace RollerCoaster.DataBase.Models.Session.Chat;

public class Message
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required int SessionId { get; set; }
    public required int SenderPlayerId { get; set; }
    public required string Text { get; set; }
    public required DateTimeOffset Time { get; set; }
}