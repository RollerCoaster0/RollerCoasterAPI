using RollerCoaster.DataTransferObjects.Session.Quests;

namespace RollerCoaster.Services.Abstractions.Sessions;

public interface IQuestStatusService
{
    Task<List<QuestStatusDTO>> GetBySession(int accessorUserId, int sessionId);
    
    Task SetStatus(int accessorUserId, int questId, QuestChangeStatusDTO questChangeStatusDto);

    Task<QuestStatusDTO> GetStatus(int accessorUserId, int questId, QuestFetchStatusDTO questFetchStatusDto);
}