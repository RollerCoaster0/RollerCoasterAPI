using Microsoft.EntityFrameworkCore;
using Minio;
using Minio.DataModel.Args;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Session;
using RollerCoaster.DataBase.Models.Session.Messages;
using RollerCoaster.DataTransferObjects.Game.NonPlayableCharacters;
using RollerCoaster.DataTransferObjects.Game.Skills;
using RollerCoaster.DataTransferObjects.LongPoll;
using RollerCoaster.DataTransferObjects.LongPoll.Updates;
using RollerCoaster.DataTransferObjects.Session.Common;
using RollerCoaster.DataTransferObjects.Session.Messages;
using RollerCoaster.DataTransferObjects.Session.Players;
using RollerCoaster.DataTransferObjects.Session.Skills;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.LongPoll;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Services.Implementations.Session;

public class PlayerService(
    DataBaseContext dataBaseContext,
    IRollService rollService,
    IFileTypeValidator fileTypeValidator,
    IMinioClient minioClient,
    ILongPollService longPollService): IPlayerService
{
    public async Task<PlayerDTO> Get(int accessorUserId, int playerId)
    {
        var player = await dataBaseContext.Players.FindAsync(playerId);
        
        if (player is null)
            throw new NotFoundError("Игрок не найден");
        
        var isUserMemberOfSession = await dataBaseContext.Players
            .AnyAsync(p => p.SessionId == player.SessionId && p.UserId == accessorUserId);
        
        var isUserGameMasterOfSession = await dataBaseContext.Sessions
            .AnyAsync(s => s.Id == player.SessionId && s.GameMasterUserId == accessorUserId);
        
        if (!isUserMemberOfSession && !isUserGameMasterOfSession)
            throw new AccessDeniedError("У вас нет доступа к этой сессии.");

        return new PlayerDTO
        {
            CharacterClassId = player.CharacterClassId,
            CurrentXPosition = player.CurrentXPosition,
            CurrentYPosition = player.CurrentYPosition,
            HealthPoints = player.HealthPoints,
            Id = player.Id,
            Name = player.Name,
            SessionId = player.SessionId,
            UserId = player.UserId,
            AvatarFilePath = player.AvatarFilePath
        };
    }

    public async Task<List<PlayerDTO>> GetBySession(int accessorUserId, int sessionId)
    {
        var isUserMemberOfSession = await dataBaseContext.Players
            .AnyAsync(p => p.SessionId == sessionId && p.UserId == accessorUserId);
        
        var isUserGameMasterOfSession = await dataBaseContext.Sessions
            .AnyAsync(s => s.Id == sessionId && s.GameMasterUserId == accessorUserId);
        
        if (!isUserMemberOfSession && !isUserGameMasterOfSession)
            throw new AccessDeniedError("У вас нет доступа к этой сессии.");

        var players = await dataBaseContext.Players
            .Where(player => player.SessionId == sessionId)
            .ToListAsync();

        return players.Select(player => new PlayerDTO
        {
            CharacterClassId = player.CharacterClassId,
            CurrentXPosition = player.CurrentXPosition,
            CurrentYPosition = player.CurrentYPosition,
            HealthPoints = player.HealthPoints,
            Id = player.Id,
            Name = player.Name,
            SessionId = player.SessionId,
            UserId = player.UserId,
            AvatarFilePath = player.AvatarFilePath
        }).ToList();
    }

    public async Task<int> Create(int accessorUserId, PlayerCreationDTO playerCreationDto)
    {
        var session = await dataBaseContext.Sessions.FindAsync(playerCreationDto.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найдена.");
        
        var isUserMemberOfSession = await dataBaseContext.Players
            .Where(p => p.SessionId == session.Id && p.UserId == accessorUserId)
            .AnyAsync();

        if (isUserMemberOfSession)
            throw new ProvidedDataIsInvalidError("Вы уже состоите в этой игре.");
        
        if (session.GameMasterUserId == accessorUserId)
            throw new ProvidedDataIsInvalidError("Вы уже состоите в этой игре в качестве ГМа.");

        var characterClass = await dataBaseContext.CharacterClasses.FindAsync(playerCreationDto.CharacterClassId);
        if (characterClass is null)
            throw new NotFoundError("Класс персонажа на найден.");
        
        if (characterClass.GameId != session.GameId)
            throw new ProvidedDataIsInvalidError("Данный класс персонажа не принадлежит этой игре.");
        
        var game = await dataBaseContext.Games.FindAsync(session.GameId);
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        var location = await dataBaseContext.Locations.FindAsync(game.BaseLocationId!.Value);
        if (location is null)
            throw new NotFoundError("Локация не найдена.");
        
        var player = new Player
        {
            UserId = accessorUserId,
            CharacterClassId = playerCreationDto.CharacterClassId,
            CurrentXPosition = location.BasePlayersXPosition!.Value,
            CurrentYPosition = location.BasePlayersYPosition!.Value,
            HealthPoints = 100,
            Name = playerCreationDto.Name,
            SessionId = playerCreationDto.SessionId,
            AvatarFilePath = null
        };
        await dataBaseContext.AddAsync(player);
        await dataBaseContext.SaveChangesAsync();
        
        var update = new PlayerJoinUpdateDTO
        {
            SessionId = session.Id,
            Player = new PlayerDTO
            {
                CharacterClassId = player.CharacterClassId,
                CurrentXPosition = player.CurrentXPosition,
                CurrentYPosition = player.CurrentYPosition,
                HealthPoints = player.HealthPoints,
                Id = player.Id,
                Name = player.Name,
                SessionId = player.SessionId,
                UserId = player.UserId,
                AvatarFilePath = player.AvatarFilePath
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
                NewMessage = null,
                Move = null,
                SessionStarted = null,
                ChangeHealthPoints = null,
                PlayerJoinUpdate = update
            });
            tasks.Add(task);
        }
        await Task.WhenAll(tasks);
        
        return player.Id;
    }

    public async Task LoadAvatar(int accessorUserId, int id, PlayerAvatarLoadDTO playerAvatarLoadDto)
    {
        var player = await dataBaseContext.Players.FindAsync(id);
        if (player is null)
            throw new NotFoundError("Игрок не найден.");
        
        if (player.UserId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этому.");
        
        if (!fileTypeValidator.ValidateImageFileType(playerAvatarLoadDto.File))
            throw new ProvidedDataIsInvalidError("Формат файла не поддерживается.");
        
        string uniqName = Guid.NewGuid().ToString("N");
        string ext = playerAvatarLoadDto.File.FileName.Split(".").Last().ToLower();
        string objectName = $"{uniqName}.{ext}";
        
        await minioClient.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket("images")
                .WithObject(objectName) 
                .WithObjectSize(playerAvatarLoadDto.File.Length)
                .WithStreamData(playerAvatarLoadDto.File.OpenReadStream()));
        
        player.AvatarFilePath = $"images/{objectName}";
        
        await dataBaseContext.SaveChangesAsync();
    }

    public async Task Move(int accessorUserId, int playerId, MoveSomeoneDTO moveSomeoneDto)
    {
        var player = await dataBaseContext.Players.FindAsync(playerId);
        if (player is null)
            throw new NotFoundError("Игрок не найден.");

        var session = await dataBaseContext.Sessions.FindAsync(player.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найден.");

        var location = await dataBaseContext.Locations.
            FirstAsync(l => l.Id == session.CurrentPlayersLocationId);

        if (moveSomeoneDto.X < 0 || moveSomeoneDto.X >= location.Width)
            throw new ProvidedDataIsInvalidError("X должен быть в диапазоне от 0 до ширины карты.");

        if (moveSomeoneDto.Y < 0 || moveSomeoneDto.Y >= location.Height)
            throw new ProvidedDataIsInvalidError("Y должен быть в диапазоне от 0 до высоты карты.");
        
        if (!session.IsActive)
            throw new ProvidedDataIsInvalidError("Игра не началась.");
        
        if (session.GameMasterUserId != accessorUserId && accessorUserId != player.UserId)
            throw new AccessDeniedError("У вас нет доступа к этому.");
        
        player.CurrentXPosition = moveSomeoneDto.X;
        player.CurrentYPosition = moveSomeoneDto.Y;

        await dataBaseContext.SaveChangesAsync();
        
        var update = new MoveUpdateDTO
        {
            SessionId = session.Id,
            ANPC = null,
            Player = new PlayerDTO
            {
                CharacterClassId = player.CharacterClassId,
                CurrentXPosition = player.CurrentXPosition,
                CurrentYPosition = player.CurrentYPosition,
                HealthPoints = player.HealthPoints,
                Id = player.Id,
                Name = player.Name,
                SessionId = player.SessionId,
                UserId = player.UserId,
                AvatarFilePath = player.AvatarFilePath
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

    public async Task ChangeHealthPoints(int accessorUserId, int playerId, ChangeHealthPointsDTO changeHealthPointsDto)
    {
        var player = await dataBaseContext.Players.FindAsync(playerId);
        if (player is null)
            throw new NotFoundError("Игрок не найден.");

        var session = await dataBaseContext.Sessions.FindAsync(player.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найден.");
        
        if (!session.IsActive)
            throw new ProvidedDataIsInvalidError("Игра не началась.");
        
        if (session.GameMasterUserId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этому.");

        if (changeHealthPointsDto.HP is not (>= 0 and <= 100))
            throw new ProvidedDataIsInvalidError("HP должно быть в диапазоне от 0 до 100");    
        
        player.HealthPoints = changeHealthPointsDto.HP;
        
        await dataBaseContext.SaveChangesAsync();
        
        var update = new ChangeHealthPointsUpdateDTO()
        {
            SessionId = session.Id,
            ANPC = null,
            Player = new PlayerDTO
            {
                CharacterClassId = player.CharacterClassId,
                CurrentXPosition = player.CurrentXPosition,
                CurrentYPosition = player.CurrentYPosition,
                HealthPoints = player.HealthPoints,
                Id = player.Id,
                Name = player.Name,
                SessionId = player.SessionId,
                UserId = player.UserId,
                AvatarFilePath = player.AvatarFilePath
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
                ChangeHealthPoints = update,
                SessionStarted = null,
                Move = null,
                PlayerJoinUpdate = null
            });
            tasks.Add(task);
        }
        await Task.WhenAll(tasks);
    }

    public async Task UseSkill(int accessorUserId, int playerId, UseSkillDTO useSkillDto)
    {
        var player = await dataBaseContext.Players.FindAsync(playerId);
        if (player is null)
            throw new NotFoundError("Игрок не найден.");

        var session = await dataBaseContext.Sessions.FindAsync(player.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найден.");
        
        if (!session.IsActive)
            throw new ProvidedDataIsInvalidError("Игра не началась.");
        
        if (session.GameMasterUserId != accessorUserId && accessorUserId != player.UserId)
            throw new AccessDeniedError("У вас нет доступа к этому.");
        
        var skill = await dataBaseContext.Skills.FindAsync(useSkillDto.SkillId);
        if (skill is null)
            throw new NotFoundError("Скилл не найден.");
        
        if (skill.GameId != session.GameId)
            throw new ProvidedDataIsInvalidError("Данный скилл не принадлежит этой игре.");
        
        var usedSkillMessage = new UsedSkillMessage
        {
            SessionId = player.SessionId,
            SenderPlayerId = player.Id,
            SenderANPCId = null,
            SkillId = useSkillDto.SkillId
        };
        await dataBaseContext.UsedSkillMessages.AddAsync(usedSkillMessage);
        await dataBaseContext.SaveChangesAsync();

        var message = new Message
        {
            SessionId = player.SessionId,
            TextMessageId = null,
            RollMessageId = null,
            UsedSkillMessageId = usedSkillMessage.Id,
            Time = DateTimeOffset.Now
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
                ANPC = null,
                Player = new PlayerDTO
                {
                    Id = player.Id,
                    UserId = player.UserId,
                    SessionId = player.SessionId,
                    Name = player.Name,
                    HealthPoints = player.HealthPoints,
                    CurrentXPosition = player.CurrentXPosition,
                    CurrentYPosition = player.CurrentYPosition,
                    CharacterClassId = player.CharacterClassId,
                    AvatarFilePath = player.AvatarFilePath
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

    public async Task<RollResultDTO> Roll(int accessorUserId, int playerId, RollDTO rollDto)
    {
        var player = await dataBaseContext.Players.FindAsync(playerId);
        if (player is null)
            throw new NotFoundError("Игрок не найден.");
        
        if (accessorUserId != player.UserId)
            throw new AccessDeniedError("У вас нет доступа к этому.");

        var session = await dataBaseContext.Sessions.FindAsync(player.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найден.");
        
        if (!session.IsActive)
            throw new ProvidedDataIsInvalidError("Игра не началась.");
        
        var rollResult = await rollService.Roll(rollDto.Die);
        
        var rollMessage = new RollMessage
        {
            SessionId = player.SessionId,
            SenderPlayerId = player.Id,
            SenderANPCId = null,
            Result = rollResult,
            Die = rollDto.Die
        };
        await dataBaseContext.RollMessages.AddAsync(rollMessage);
        await dataBaseContext.SaveChangesAsync();

        var message = new Message
        {
            SessionId = player.SessionId,
            TextMessageId = null,
            RollMessageId = rollMessage.Id,
            UsedSkillMessageId = null,
            Time = DateTimeOffset.Now
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
                SenderANPC = null,
                SenderPlayer = new PlayerDTO
                {
                    Id = player.Id,
                    UserId = player.UserId,
                    SessionId = player.SessionId,
                    Name = player.Name,
                    HealthPoints = player.HealthPoints,
                    CurrentXPosition = player.CurrentXPosition,
                    CurrentYPosition = player.CurrentYPosition,
                    CharacterClassId = player.CharacterClassId,
                    AvatarFilePath = player.AvatarFilePath
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