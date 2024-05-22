using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Session;
using RollerCoaster.DataTransferObjects.Session;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Services.Realisations.Session;

public class QuestStatusService(DataBaseContext dataBaseContext): IQuestStatusService
{
    public async Task SetStatus(int accessorUserId, int questId, QuestChangeStatusDTO questChangeStatusDto)
    {
        var quest = await dataBaseContext.Quests.FindAsync(questId);
        if (quest is null)
            throw new NotFoundError("Квест не найден");
        
        var session = await dataBaseContext.Sessions.FindAsync(questChangeStatusDto.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найдена");
        
        if (session.GameMasterId != accessorUserId)
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
        // TODO: получать статус квеста может только участник игры, валидировать это
        
        var questStatus = await dataBaseContext.QuestStatuses.FindAsync(questFetchStatusDto.SessionId, questId);
        if (questStatus is null)
            throw new NotFoundError("Пока у этого квеста нет статуса");

        return new QuestStatusDTO
        {
            Status = questStatus.Status
        };
    }
}