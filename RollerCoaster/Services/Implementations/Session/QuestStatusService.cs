using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Session;
using RollerCoaster.DataTransferObjects.Game.Quests;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Services.Implementations.Session;

public class QuestStatusService(DataBaseContext dataBaseContext): IQuestStatusService
{
    public async Task SetStatus(
        int accessorUserId, 
        int questId, 
        QuestChangeStatusDTO questChangeStatusDto)
    {
        var quest = await dataBaseContext.Quests.FindAsync(questId);
        if (quest is null)
            throw new NotFoundError("Квест не найден");
        
        var session = await dataBaseContext.Sessions.FindAsync(questChangeStatusDto.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найдена");
        
        if (session.GameMasterUserId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этому.");
        
        var questStatus = await dataBaseContext.QuestStatuses.FindAsync(questChangeStatusDto.SessionId, questId);

        if (questStatus is null)
        {
            questStatus = new QuestStatus
            {
                SessionId = questChangeStatusDto.SessionId,
                QuestId = questId,
                Status = questChangeStatusDto.Status
            };
            await dataBaseContext.AddAsync(questStatus);
            
        }
        else
        {
            questStatus.Status = questChangeStatusDto.Status;
        }
            
        await dataBaseContext.SaveChangesAsync();
    }

    public async Task<QuestStatusDTO> GetStatus(
        int accessorUserId, 
        int questId, 
        QuestFetchStatusDTO questFetchStatusDto)
    {
        var questStatus = await dataBaseContext.QuestStatuses.FindAsync(questFetchStatusDto.SessionId, questId);
        if (questStatus is null)
            throw new NotFoundError("Пока у этого квеста нет статуса");
        
        var isUserMemberOfSession = await dataBaseContext.Players
            .AnyAsync(p => p.SessionId == questStatus.SessionId && p.UserId == accessorUserId);
        
        var isUserGameMasterOfSession = await dataBaseContext.Sessions
            .AnyAsync(s => s.Id == questStatus.SessionId && s.GameMasterUserId == accessorUserId);
        
        if (!isUserMemberOfSession && !isUserGameMasterOfSession)
            throw new AccessDeniedError("У вас нет доступа к этой сессии.");

        return new QuestStatusDTO
        {
            Status = questStatus.Status
        };
    }
}