using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Session.Chat;
using RollerCoaster.DataTransferObjects.Chat;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Services.Realisations.Session;

public class ChatService(DataBaseContext dataBaseContext) : IChatService
{
    public async Task<int> SendMessage(int senderId, SendMessageDTO sendMessageDto)
    {
        if (sendMessageDto.Text.Length > 512)
            throw new ProvidedDataIsInvalidError("Сообщение слишком длинное");

        var message = new Message
        {
            SessionId = sendMessageDto.SessionId, // TODO:add validation
            SenderId = senderId,
            Text = sendMessageDto.Text,
            Time = DateTimeOffset.Now
        };
        await dataBaseContext.Messages.AddAsync(message);
        await dataBaseContext.SaveChangesAsync();

        return message.Id;
    }

    public async Task<List<MessageDTO>> GetLastMessages(GetLastMessagesDTO getLastMessagesDto)
    {
        if (getLastMessagesDto.Count > 200)
            throw new ProvidedDataIsInvalidError("Число сообщений слишком больше");

        var messages = await dataBaseContext.Messages
            .Where(m => m.SessionId == getLastMessagesDto.SessionId) // TODO:add validation
            .OrderByDescending(m => m.Id)
            .Take(getLastMessagesDto.Count)
            .OrderBy(m => m.Id)
            .ToListAsync();

        return messages.Select(m => new MessageDTO 
        { 
            MessageId = m.Id,
            SenderId = m.SenderId,
            SessionId = m.SessionId,
            Text = m.Text,
            Time = m.Time
        }).ToList();
    }
}