using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Session.Messages;
using RollerCoaster.DataTransferObjects.Game.Skills;
using RollerCoaster.DataTransferObjects.LongPoll;
using RollerCoaster.DataTransferObjects.LongPoll.Updates;
using RollerCoaster.DataTransferObjects.Session.ActiveNonPlayableCharacter;
using RollerCoaster.DataTransferObjects.Session.Common;
using RollerCoaster.DataTransferObjects.Session.Messages;
using RollerCoaster.DataTransferObjects.Session.Skills;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.LongPoll;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Services.Implementations.Session;

public class ActiveNonPlayableCharactersService(
    DataBaseContext dataBaseContext, 
    IRollService rollService,
    ILongPollService longPollService): IActiveNonPlayableCharactersService
{
    public async Task<ActiveNonPlayableCharacterDTO> Get(int accessorUserId, int anpcId)
    {
        var anpc = await dataBaseContext.ActiveNonPlayableCharacters.FindAsync(anpcId);
        if (anpc is null)
            throw new NotFoundError("NPC не найден.");
        
        var isUserMemberOfSession = await dataBaseContext.Players
            .AnyAsync(p => p.SessionId == anpc.SessionId && p.UserId == accessorUserId);
        
        var isUserGameMasterOfSession = await dataBaseContext.Sessions
            .AnyAsync(s => s.Id == anpc.SessionId && s.GameMasterUserId == accessorUserId);
        
        if (!isUserMemberOfSession && !isUserGameMasterOfSession)
            throw new AccessDeniedError("У вас нет доступа к этой сессии.");

        return new ActiveNonPlayableCharacterDTO
        {
            NonPlayableCharacterId = anpc.NonPlayableCharacterId,
            CurrentXPosition = anpc.CurrentXPosition,
            CurrentYPosition = anpc.CurrentYPosition,
            HealthPoints = anpc.HealthPoints,
            Id = anpc.Id,
            SessionId = anpc.SessionId
        };
    }

    public async Task<List<ActiveNonPlayableCharacterDTO>> GetBySession(int accessorUserId, int sessionId)
    {
        var isUserMemberOfSession = await dataBaseContext.Players
            .AnyAsync(p => p.SessionId == sessionId && p.UserId == accessorUserId);
        
        var isUserGameMasterOfSession = await dataBaseContext.Sessions
            .AnyAsync(s => s.Id == sessionId && s.GameMasterUserId == accessorUserId);
        
        if (!isUserMemberOfSession && !isUserGameMasterOfSession)
            throw new AccessDeniedError("У вас нет доступа к этой сессии.");

        var anpcs = await dataBaseContext.ActiveNonPlayableCharacters
            .Where(anpc => anpc.SessionId == sessionId)
            .ToListAsync();

        return anpcs.Select(anpc => new ActiveNonPlayableCharacterDTO
        {
            NonPlayableCharacterId = anpc.NonPlayableCharacterId,
            CurrentXPosition = anpc.CurrentXPosition,
            CurrentYPosition = anpc.CurrentYPosition,
            HealthPoints = anpc.HealthPoints,
            Id = anpc.Id,
            SessionId = anpc.SessionId
        }).ToList();
    }

    public async Task Move(int accessorUserId, int anpcId, MoveSomeoneDTO moveSomeoneDto)
    {
        var anpc = await dataBaseContext.ActiveNonPlayableCharacters.FindAsync(anpcId);
        if (anpc is null)
            throw new NotFoundError("NPC не найден.");

        var session = await dataBaseContext.Sessions.FindAsync(anpc.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найден.");

        var npc = await dataBaseContext.NonPlayableCharacters.
            FirstAsync(d => d.Id == anpc.NonPlayableCharacterId);

        var location = await dataBaseContext.Locations.
            FirstAsync(l => l.Id == npc.BaseLocationId);
        
        if (moveSomeoneDto.X < 0 || moveSomeoneDto.X >= location.Width)
            throw new ProvidedDataIsInvalidError("X должен быть в диапазоне от 0 до ширины карты.");

        if (moveSomeoneDto.Y < 0 || moveSomeoneDto.Y >= location.Height)
            throw new ProvidedDataIsInvalidError("Y должен быть в диапазоне от 0 до высоты карты.");

        if (!session.IsActive)
            throw new ProvidedDataIsInvalidError("Игра не началась.");
        
        if (session.GameMasterUserId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этому.");
        
        anpc.CurrentXPosition = moveSomeoneDto.X;
        anpc.CurrentYPosition = moveSomeoneDto.Y;

        await dataBaseContext.SaveChangesAsync();
        
        var update = new MoveUpdateDTO
        {
            SessionId = session.Id,
            Player = null,
            ANPC = new ActiveNonPlayableCharacterDTO
            {
                NonPlayableCharacterId = anpc.NonPlayableCharacterId,
                CurrentXPosition = anpc.CurrentXPosition,
                CurrentYPosition = anpc.CurrentYPosition,
                HealthPoints = anpc.HealthPoints,
                Id = anpc.Id,
                SessionId = anpc.SessionId
            },
            Y = moveSomeoneDto.Y,
            X = moveSomeoneDto.X
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
                NewMessage = null,
                Move = update,
                SessionStarted = null,
                ChangeHealthPoints = null,
                PlayerJoinUpdate = null
            });
            tasks.Add(task);
        }
        await Task.WhenAll(tasks);
    }

    public async Task ChangeHealthPoints(int accessorUserId, int anpcId, ChangeHealthPointsDTO changeHealthPointsDto)
    {
        var anpc = await dataBaseContext.ActiveNonPlayableCharacters.FindAsync(anpcId);
        if (anpc is null)
            throw new NotFoundError("NPC не найден.");

        var session = await dataBaseContext.Sessions.FindAsync(anpc.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найден.");
        
        if (!session.IsActive)
            throw new ProvidedDataIsInvalidError("Игра не началась.");
        
        if (session.GameMasterUserId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этому.");
        
        if (changeHealthPointsDto.HP is not (>= 0 and <= 100))
            throw new ProvidedDataIsInvalidError("HP должно быть в диапазоне от 0 до 100");  

        anpc.HealthPoints = changeHealthPointsDto.HP;
        
        await dataBaseContext.SaveChangesAsync();
        
        var update = new ChangeHealthPointsUpdateDTO
        {
            SessionId = session.Id,
            Player = null,
            ANPC = new ActiveNonPlayableCharacterDTO
            {
                NonPlayableCharacterId = anpc.NonPlayableCharacterId,
                CurrentXPosition = anpc.CurrentXPosition,
                CurrentYPosition = anpc.CurrentYPosition,
                HealthPoints = anpc.HealthPoints,
                Id = anpc.Id,
                SessionId = anpc.SessionId
            },
            NewHP = changeHealthPointsDto.HP
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
                NewMessage = null,
                Move = null,
                SessionStarted = null,
                ChangeHealthPoints = update,
                PlayerJoinUpdate = null
            });
            tasks.Add(task);
        }
        await Task.WhenAll(tasks);
    }

    public async Task UseSkill(int accessorUserId, int anpcId, UseSkillDTO useSkillDto)
    {
        var anpc = await dataBaseContext.ActiveNonPlayableCharacters.FindAsync(anpcId);
        if (anpc is null)
            throw new NotFoundError("NPC не найден.");

        var session = await dataBaseContext.Sessions.FindAsync(anpc.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найден.");
        
        if (!session.IsActive)
            throw new ProvidedDataIsInvalidError("Игра не началась.");
        
        if (session.GameMasterUserId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этому.");
        
        var skill = await dataBaseContext.Skills.FindAsync(useSkillDto.SkillId);
        
        if (skill is null)
            throw new NotFoundError("Скилл не найден.");

        if (skill.GameId != session.GameId)
            throw new ProvidedDataIsInvalidError("Данный скилл не принадлежит этой игре.");
        
        var usedSkillMessage = new UsedSkillMessage
        {
            SessionId = anpc.SessionId,
            SenderPlayerId = null,
            SenderANPCId = anpc.Id,
            SkillId = useSkillDto.SkillId
        };
        await dataBaseContext.UsedSkillMessages.AddAsync(usedSkillMessage);
        await dataBaseContext.SaveChangesAsync();

        var message = new Message
        {
            SessionId = anpc.SessionId,
            Time = DateTimeOffset.Now,
            TextMessageId = null,
            RollMessageId = null,
            UsedSkillMessageId = usedSkillMessage.Id
        };
        await dataBaseContext.Messages.AddAsync(message);
        await dataBaseContext.SaveChangesAsync();
        
        var update = new MessageDTO
        {
            Id = message.Id,
            SessionId = session.Id,
            Time = message.Time,
            TextMessage = null,
            RollMessage = null,
            UsedSkillMessage = new UsedSkillMessageDTO
            {
                Player = null,
                ANPC = new ActiveNonPlayableCharacterDTO
                {
                    NonPlayableCharacterId = anpc.NonPlayableCharacterId,
                    CurrentXPosition = anpc.CurrentXPosition,
                    CurrentYPosition = anpc.CurrentYPosition,
                    HealthPoints = anpc.HealthPoints,
                    Id = anpc.Id,
                    SessionId = anpc.SessionId
                },
                Skill = new SkillDTO
                {
                    Id = skill.Id,
                    GameId = skill.GameId,
                    Name = skill.Name,
                    Description = skill.Description,
                    AvailableOnlyForCharacterClassId = skill.AvailableOnlyForCharacterClassId,
                    AvailableOnlyForNonPlayableCharacterId = skill.AvailableOnlyForNonPlayableCharacterId
                }
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
                SessionStarted = null,
                ChangeHealthPoints = null,
                PlayerJoinUpdate = null
            });
            tasks.Add(task);
        }
        await Task.WhenAll(tasks);
    }
    
    public async Task<RollResultDTO> Roll(int accessorUserId, int anpcId, RollDTO rollDto)
    {
        var anpc = await dataBaseContext.ActiveNonPlayableCharacters.FindAsync(anpcId);
        if (anpc is null)
            throw new NotFoundError("NPC не найден.");

        var session = await dataBaseContext.Sessions.FindAsync(anpc.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найден.");
        
        if (!session.IsActive)
            throw new ProvidedDataIsInvalidError("Игра не началась.");
        
        if (session.GameMasterUserId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этому.");
        
        var rollResult = await rollService.Roll(rollDto.Die);
        
        var rollMessage = new RollMessage
        {
            SessionId = anpc.SessionId,
            SenderPlayerId = null,
            SenderANPCId = anpc.Id,
            Result = rollResult,
            Die = rollDto.Die
        };
        await dataBaseContext.RollMessages.AddAsync(rollMessage);
        await dataBaseContext.SaveChangesAsync();

        var message = new Message
        {
            SessionId = anpc.Id,
            Time = DateTimeOffset.Now,
            TextMessageId = null,
            RollMessageId = rollMessage.Id,
            UsedSkillMessageId = null
        };
        await dataBaseContext.Messages.AddAsync(message);
        await dataBaseContext.SaveChangesAsync();
        
        var update = new MessageDTO
        {
            Id = message.Id,
            SessionId = session.Id,
            Time = message.Time,
            TextMessage = null,
            UsedSkillMessage = null,
            RollMessage = new RollMessageDTO
            {
                SenderPlayer = null,
                SenderANPC = new ActiveNonPlayableCharacterDTO
                {
                    NonPlayableCharacterId = anpc.NonPlayableCharacterId,
                    CurrentXPosition = anpc.CurrentXPosition,
                    CurrentYPosition = anpc.CurrentYPosition,
                    HealthPoints = anpc.HealthPoints,
                    Id = anpc.Id,
                    SessionId = anpc.SessionId
                },
                Result = new RollResultDTO
                {
                    Result = rollResult,
                    Die = rollDto.Die
                }
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
                SessionStarted = null,
                ChangeHealthPoints = null,
                PlayerJoinUpdate = null
            });
            tasks.Add(task);
        }
        await Task.WhenAll(tasks);
        
        return new RollResultDTO
        {
            Result = rollResult,
            Die = rollDto.Die
        };
    }
}