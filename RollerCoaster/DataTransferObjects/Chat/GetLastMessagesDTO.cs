using System.ComponentModel.DataAnnotations;

namespace RollerCoaster.DataTransferObjects.Chat;

public class GetLastMessagesDTO
{
    [Range(1, 100)] public required int Count { get; set; }
    public required int SessionId { get; set; }
}