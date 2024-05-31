using System.ComponentModel.DataAnnotations;

namespace RollerCoaster.DataTransferObjects.Session.Messages;

public class SendTextMessageDTO
{
    [StringLength(512)] public required string Text { get; set; }
    public required int SessionId { get; set; }
}