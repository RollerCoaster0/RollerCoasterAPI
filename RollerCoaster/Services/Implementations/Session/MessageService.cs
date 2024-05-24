using RollerCoaster.DataTransferObjects.Session.Chat;
using RollerCoaster.DataTransferObjects.Session.Chat.Messages;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Services.Implementations.Session;

public class MessageService: IMessageService
{
    public async Task<MessageDTO> Get(int accessorUserId, int messageId)
    {
        throw new NotImplementedException();
    }

    public async Task<int> Create(int senderUserId, SendMessageDTO sendMessageDto)
    {
        /*
        var message = new Message
        {
            SessionId = sendMessageDto.SessionId, // TODO:add validation
            SenderUserId = senderUserId,
            Text = sendMessageDto.Text,
            Time = DateTimeOffset.Now
        };
        await dataBaseContext.Messages.AddAsync(message);
        await dataBaseContext.SaveChangesAsync();

        return message.Id;
        */
    
        throw new NotImplementedException();
    }
}