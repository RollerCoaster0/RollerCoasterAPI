using System.ComponentModel.DataAnnotations;

namespace RollerCoaster.DataTransferObjects.Session.Messages;

public class GetMessagesDTO
{
    public required int SessionId { get; set; }
    [Range(1, 400)] public required int Count { get; set; }
}