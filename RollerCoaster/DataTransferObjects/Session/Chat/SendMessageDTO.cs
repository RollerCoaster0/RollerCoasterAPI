using System.ComponentModel.DataAnnotations;

namespace RollerCoaster.DataTransferObjects.Session.Chat;

public class SendMessageDTO
{
    [StringLength(512)] public required string Text { get; set; }
    public required int SessionId { get; set; }
}