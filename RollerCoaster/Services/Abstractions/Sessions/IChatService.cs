using RollerCoaster.DataTransferObjects.Chat;

namespace RollerCoaster.Services.Abstractions.Sessions;

public class UserNotFoundError(string message): Exception(message);

public interface IChatService
{
    public Task<int> SendMessage(int senderId, SendMessageDTO sendMessageDto);

    public Task<List<MessageDTO>> GetLastMessages(GetLastMessagesDTO getLastMessagesDto);
}