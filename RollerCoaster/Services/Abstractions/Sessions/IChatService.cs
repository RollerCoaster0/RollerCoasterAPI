using RollerCoaster.DataTransferObjects.Chat;

namespace RollerCoaster.Services.Abstractions.Sessions;

public interface IChatService
{
    public Task<int> SendMessage(int senderId, SendMessageDTO sendMessageDto);

    public Task<List<MessageDTO>> GetLastMessages(GetLastMessagesDTO getLastMessagesDto);
}