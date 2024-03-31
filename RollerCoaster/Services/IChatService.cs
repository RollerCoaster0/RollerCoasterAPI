using RollerCoaster.DataTransferObjects.Chat;

namespace RollerCoaster.Services;

public class UserNotFoundError(string message): Exception(message)
{
    
}

public class MessageInvalidError(string message) : Exception(message)
{
    
}

public class InvalidMessagesCountError(string message) : Exception(message)
{
    
}
public interface IChatService
{
    public Task<int> SendMessage(int senderId, SendMessageDTO sendMessageDto);

    public Task<List<MessageDTO>> GetLastMessages(GetLastMessagesDTO getLastMessagesDto);
}