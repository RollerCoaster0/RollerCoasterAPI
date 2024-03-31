namespace RollerCoaster.DataTransferObjects.Chat;

public class GetLastMessagesDTO
{
    public required int Count { get; set; }
    public required int SessionId { get; set; }
}