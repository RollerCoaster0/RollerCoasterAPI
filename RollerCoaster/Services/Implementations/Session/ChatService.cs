using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase;
using RollerCoaster.DataTransferObjects.Session.Chat;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Services.Implementations.Session;

public class ChatService(DataBaseContext dataBaseContext) : IChatService
{
    public async Task<List<ChatActionDTO>> GetActions(int accessorUserId, GetChatActionsDTO getChatActionsDto)
    {
        var session = await dataBaseContext.Sessions.FindAsync(getChatActionsDto.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найдена.");
        
        var isUserMemberOfSession = await dataBaseContext.Players
            .Where(p => p.SessionId == session.Id && p.UserId == accessorUserId)
            .AnyAsync();
        
        var isUserGameMasterOfSession = session.GameMasterUserId == accessorUserId;
        
        if (!isUserMemberOfSession && !isUserGameMasterOfSession && session.IsActive)
            throw new AccessDeniedError("У вас нет доступа к этой сессии.");

        var usedRollActions = await dataBaseContext.UsedRollActions
            .Where(a => a.SessionId == getChatActionsDto.SessionId)
            .ToArrayAsync();
        
        var usedSkillsActions = await dataBaseContext.UsedSkillActions
            .Where(a => a.SessionId == getChatActionsDto.SessionId)
            .ToArrayAsync();

        var messagesActions = await dataBaseContext.Messages
            .Where(m => m.SessionId == getChatActionsDto.SessionId)
            .ToArrayAsync();
        
        // TODO: сложить все эти экшены в список из ChatActionDTO,
        // а затем отсортировать по дате

        return [];
    }
}