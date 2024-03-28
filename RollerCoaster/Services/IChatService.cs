using RollerCoaster.DataTransferObjects.Chat;

namespace RollerCoaster.Services;

public interface IChatService
{
    public Task<int> SendMessage(int senderId, SendMessageDTO sendMessageDto);

    public Task<List<MessageDTO>> GetLastMessages(GetLastMessagesDTO getLastMessagesDto);
}