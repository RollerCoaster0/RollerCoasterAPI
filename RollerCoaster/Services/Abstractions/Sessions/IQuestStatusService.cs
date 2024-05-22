using RollerCoaster.DataTransferObjects.Session;

namespace RollerCoaster.Services.Abstractions.Sessions;

public interface IQuestStatusService
{
    Task SetStatus(int accessorUserId, int questId, QuestChangeStatusDTO questChangeStatusDto);

    Task<QuestStatusDTO> GetStatus(int accessorUserId, int questId, QuestFetchStatusDTO questFetchStatusDto);
}