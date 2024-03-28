using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Chat;
using RollerCoaster.DataTransferObjects.Chat;

namespace RollerCoaster.Services;

public class ChatService(DataBaseContext dataBaseContext) : IChatService
{
    public async Task<int> SendMessage(int senderId, SendMessageDTO sendMessageDto)
    {
        var message = new Message()
        {
            SessionId = sendMessageDto.SessionId, // TODO:add validation
            SenderId = senderId,//TODO: add valid
            Text = sendMessageDto.Text, //TODO: add valid
            Time = DateTimeOffset.Now
        };
        await dataBaseContext.Messages.AddAsync(message);
        await dataBaseContext.SaveChangesAsync();

        return message.Id;
    }

    public async Task<List<MessageDTO>> GetLastMessages(GetLastMessagesDTO getLastMessagesDto)
    {
        var messages = await dataBaseContext.Messages
            .Where(m => m.SessionId == getLastMessagesDto.SessionId)// TODO:add validation
            .Take(getLastMessagesDto.Count)// TODO:add validation
            .ToListAsync();//TODO: add sort
        var messagesDto = messages.Select(m => new MessageDTO()
        {
            MessageId = m.Id,
            SenderId = m.SenderId,
            SessionId = m.SessionId,
            Text = m.Text,
            Time = m.Time
        }).ToList();
        return messagesDto;
    }
}