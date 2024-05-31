using RollerCoaster.DataTransferObjects.Session.Messages;

namespace RollerCoaster.Services.Abstractions.Sessions;

public interface IMessageService
{
    public Task<int> SendTextMessage(int senderUserId, SendTextMessageDTO sendTextMessageDto);
    
    public Task<List<MessageDTO>> GetBySession(int accessorUserId, GetMessagesDTO getMessagesDto);
}