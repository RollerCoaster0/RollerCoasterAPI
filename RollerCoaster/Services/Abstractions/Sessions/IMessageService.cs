using RollerCoaster.DataTransferObjects.Session.Chat;
using RollerCoaster.DataTransferObjects.Session.Chat.Messages;

namespace RollerCoaster.Services.Abstractions.Sessions;

public interface IMessageService
{
    public Task<MessageDTO> Get(int accessorUserId, int messageId);
    
    public Task<int> Create(int senderUserId, SendMessageDTO sendMessageDto);
}