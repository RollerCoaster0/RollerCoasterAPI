using RollerCoaster.DataTransferObjects.Session;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Services.Realisations.Session;

public class QuestStatusService: IQuestStatusService
{
    public Task SetStatus(int accessorUserId, int questId, QuestChangeStatusDTO questChangeStatusDto)
    {
        throw new NotImplementedException();
    }

    public Task<QuestStatusDTO> GetStatus(int accessorUserId, int questId, QuestFetchStatusDTO questFetchStatusDto)
    {
        throw new NotImplementedException();
    }
}