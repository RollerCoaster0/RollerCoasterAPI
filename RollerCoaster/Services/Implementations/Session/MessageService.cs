using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Session.Messages;
using RollerCoaster.DataTransferObjects.Game.Skills;
using RollerCoaster.DataTransferObjects.LongPoll;
using RollerCoaster.DataTransferObjects.Session.ActiveNonPlayableCharacter;
using RollerCoaster.DataTransferObjects.Session.Common;
using RollerCoaster.DataTransferObjects.Session.Messages;
using RollerCoaster.DataTransferObjects.Session.Players;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.LongPoll;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Services.Implementations.Session;

public class MessageService(
    DataBaseContext dataBaseContext,
    ILongPollService longPollService): IMessageService
{
    public async Task<int> SendTextMessage(int senderUserId, SendTextMessageDTO sendTextMessageDto)
    {
        var session = await dataBaseContext.Sessions.FindAsync(sendTextMessageDto.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найдена.");

        var player = await dataBaseContext.Players
            .FirstOrDefaultAsync(p => p.SessionId == session.Id && p.UserId == senderUserId);

        if (player is null)
            throw new NotFoundError("Вы не можете отправлять сообщения в этой игре.");

        var textMessage = new TextMessage
        {
            SessionId = sendTextMessageDto.SessionId,
            SenderPlayerId = player.Id,
            Text = sendTextMessageDto.Text,
            Time = DateTimeOffset.Now
        };
        await dataBaseContext.TextMessages.AddAsync(textMessage);
        await dataBaseContext.SaveChangesAsync();

        var message = new Message
        {
            SessionId = sendTextMessageDto.SessionId,
            TextMessageId = textMessage.Id,
            RollMessageId = null,
            UsedSkillMessageId = null
        };
        await dataBaseContext.Messages.AddAsync(message);
        await dataBaseContext.SaveChangesAsync();
        
        var update = new MessageDTO
        {
            Id = message.Id,
            SessionId = session.Id,
            UsedSkillMessage = null,
            RollMessage = null,
            TextMessage = new TextMessageDTO
            {
                SenderPlayer = new PlayerDTO
                {
                    Id = player.Id,
                    UserId = player.UserId,
                    SessionId = player.SessionId,
                    Name = player.Name,
                    Level = player.Level,
                    HealthPoints = player.HealthPoints,
                    CurrentXPosition = player.CurrentXPosition,
                    CurrentYPosition = player.CurrentYPosition,
                    CharacterClassId = player.CharacterClassId
                },
                Text = textMessage.Text,
                Time = textMessage.Time
            }
        };

        var membersOfSessionUserIds = await dataBaseContext.Players
            .Where(p => p.SessionId == session.Id)
            .Select(p => p.UserId)
            .ToListAsync();
        membersOfSessionUserIds.Add(session.GameMasterUserId);

        var tasks = new List<Task>();
        foreach (var userId in membersOfSessionUserIds)
        {
            Task task = longPollService.EnqueueUpdateAsync(userId, new LongPollUpdate
            {
                QuestStatusUpdate = null,
                NewMessage = update,
                Move = null,
                SessionStarted = null
            });
            tasks.Add(task);
        }
        await Task.WhenAll(tasks);
        
        return message.Id;
    }

    public async Task<List<MessageDTO>> GetBySession(int accessorUserId, GetMessagesDTO getMessagesDto)
    {
        var session = await dataBaseContext.Sessions.FindAsync(getMessagesDto.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найдена.");
        
        var isUserMemberOfSession = await dataBaseContext.Players
            .Where(p => p.SessionId == session.Id && p.UserId == accessorUserId)
            .AnyAsync();
        
        var isUserGameMasterOfSession = session.GameMasterUserId == accessorUserId;
        
        if (!isUserMemberOfSession && !isUserGameMasterOfSession)
            throw new AccessDeniedError("У вас нет доступа к этой сессии.");

        var messages = await dataBaseContext.Messages
            .Where(m => m.SessionId == getMessagesDto.SessionId)
            
            .OrderByDescending(m => m.Id) // first element is last message
            .Take(getMessagesDto.Count)
            
            .Include(m => m.TextMessage)
            .ThenInclude(m => m!.SenderPlayer)
            
            .Include(m => m.RollMessage)
            .ThenInclude(m => m!.SenderPlayer)
            .Include(m => m.RollMessage)
            .ThenInclude(m => m!.SenderANPC)
            
            .Include(m => m.UsedSkillMessage)
            .ThenInclude(m => m!.SenderPlayer)
            .Include(m => m.UsedSkillMessage)
            .ThenInclude(m => m!.SenderANPC)
            .Include(m => m.UsedSkillMessage)
            .ThenInclude(m => m!.Skill)
            
            .ToArrayAsync();
        
        // TODO: отрефакторить
        
        return messages.Select(m => new MessageDTO
        {
            Id =  m.Id,
            SessionId = m.SessionId,
            
            RollMessage = m.RollMessage is null ? null : new RollMessageDTO
            {
                SenderPlayer = m.RollMessage.SenderPlayer is null ? null : new PlayerDTO
                {
                    Id = m.RollMessage.SenderPlayer.Id,
                    UserId = m.RollMessage.SenderPlayer.UserId,
                    SessionId = m.RollMessage.SenderPlayer.SessionId,
                    Name = m.RollMessage.SenderPlayer.Name,
                    Level = m.RollMessage.SenderPlayer.Level,
                    HealthPoints = m.RollMessage.SenderPlayer.HealthPoints,
                    CurrentXPosition = m.RollMessage.SenderPlayer.CurrentXPosition,
                    CurrentYPosition = m.RollMessage.SenderPlayer.CurrentYPosition,
                    CharacterClassId = m.RollMessage.SenderPlayer.CharacterClassId
                },
                SenderANPC = m.RollMessage.SenderANPC is null ? null : new ActiveNonPlayableCharacterDTO
                {
                    Id = m.RollMessage.SenderANPC.Id,
                    NonPlayableCharacterId = m.RollMessage.SenderANPC.NonPlayableCharacterId,
                    SessionId = m.RollMessage.SenderANPC.SessionId,
                    HealthPoints = m.RollMessage.SenderANPC.HealthPoints,
                    CurrentXPosition = m.RollMessage.SenderANPC.CurrentXPosition,
                    CurrentYPosition = m.RollMessage.SenderANPC.CurrentYPosition,
                },
                Result = new RollResultDTO
                {
                    Result = m.RollMessage.Result,
                    Die = m.RollMessage.Die
                }
            },
            
            UsedSkillMessage = m.UsedSkillMessage is null ? null : new UsedSkillMessageDTO
            {
                Player = m.UsedSkillMessage.SenderPlayer is null ? null : new PlayerDTO
                {
                    Id = m.UsedSkillMessage.SenderPlayer.Id,
                    UserId = m.UsedSkillMessage.SenderPlayer.UserId,
                    SessionId = m.UsedSkillMessage.SenderPlayer.SessionId,
                    Name = m.UsedSkillMessage.SenderPlayer.Name,
                    Level = m.UsedSkillMessage.SenderPlayer.Level,
                    HealthPoints = m.UsedSkillMessage.SenderPlayer.HealthPoints,
                    CurrentXPosition = m.UsedSkillMessage.SenderPlayer.CurrentXPosition,
                    CurrentYPosition = m.UsedSkillMessage.SenderPlayer.CurrentYPosition,
                    CharacterClassId = m.UsedSkillMessage.SenderPlayer.CharacterClassId
                },
                ANPC = m.UsedSkillMessage.SenderANPC is null ? null : new ActiveNonPlayableCharacterDTO
                {
                    Id = m.UsedSkillMessage.SenderANPC.Id,
                    NonPlayableCharacterId = m.UsedSkillMessage.SenderANPC.NonPlayableCharacterId,
                    SessionId = m.UsedSkillMessage.SenderANPC.SessionId,
                    HealthPoints = m.UsedSkillMessage.SenderANPC.HealthPoints,
                    CurrentXPosition = m.UsedSkillMessage.SenderANPC.CurrentXPosition,
                    CurrentYPosition = m.UsedSkillMessage.SenderANPC.CurrentYPosition,
                },
                Skill = new SkillDTO
                {
                    Id = m.UsedSkillMessage.SkillId,
                    GameId = m.UsedSkillMessage.Skill.GameId,
                    Name = m.UsedSkillMessage.Skill.Name,
                    Description = m.UsedSkillMessage.Skill.Description,
                    AvailableOnlyForCharacterClassId = m.UsedSkillMessage.Skill.AvailableOnlyForCharacterClassId,
                    AvailableOnlyForNonPlayableCharacterId = m.UsedSkillMessage.Skill.AvailableOnlyForNonPlayableCharacterId
                }
            },
            
            TextMessage = m.TextMessage is null ? null : new TextMessageDTO
            {
                Text = m.TextMessage.Text,
                Time = m.TextMessage.Time,
                SenderPlayer = new PlayerDTO
                {
                    Id = m.TextMessage.SenderPlayer.Id,
                    UserId = m.TextMessage.SenderPlayer.UserId,
                    SessionId = m.TextMessage.SenderPlayer.SessionId,
                    Name = m.TextMessage.SenderPlayer.Name,
                    Level = m.TextMessage.SenderPlayer.Level,
                    HealthPoints = m.TextMessage.SenderPlayer.HealthPoints,
                    CurrentXPosition = m.TextMessage.SenderPlayer.CurrentXPosition,
                    CurrentYPosition = m.TextMessage.SenderPlayer.CurrentYPosition,
                    CharacterClassId = m.TextMessage.SenderPlayer.CharacterClassId
                }
            }
        })
        .ToList();
    }
}