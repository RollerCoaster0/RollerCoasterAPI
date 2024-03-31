using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Chat;
using RollerCoaster.DataTransferObjects.Chat;

namespace RollerCoaster.Services;

public class ChatService(DataBaseContext dataBaseContext) : IChatService
{
    public async Task<int> SendMessage(int senderId, SendMessageDTO sendMessageDto)
    {
        if (await dataBaseContext.Users.FindAsync(senderId) is null)
        {
            throw new UserNotFoundError("Пользователь не найден");
        }

        if (sendMessageDto.Text.Length > 512)
        {
            throw new MessageInvalidError("Сообщение слишком длинное");
        }

        var message = new Message()
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
        {
            throw new InvalidMessagesCountError("Число сообщений слишком больше");
        }

        var messages = await dataBaseContext.Messages
            .Where(m => m.SessionId == getLastMessagesDto.SessionId) // TODO:add validation
            .OrderByDescending(m => m.Id)
            .Take(getLastMessagesDto.Count)
            .OrderBy(m => m.Id)
            .ToListAsync();

        var messagesDto = messages
            .Select(m => new MessageDTO()
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