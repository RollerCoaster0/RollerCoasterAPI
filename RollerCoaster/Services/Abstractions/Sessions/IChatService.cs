using RollerCoaster.DataTransferObjects.Session.Chat;

namespace RollerCoaster.Services.Abstractions.Sessions;

public interface IChatService
{
    public Task<int> SendMessage(int senderUserId, SendMessageDTO sendMessageDto);

    public Task<List<MessageDTO>> GetLastMessages(GetLastMessagesDTO getLastMessagesDto);
}