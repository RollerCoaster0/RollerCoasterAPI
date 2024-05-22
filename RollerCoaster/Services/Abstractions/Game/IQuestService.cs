using RollerCoaster.DataTransferObjects.Game.Quests;

namespace RollerCoaster.Services.Abstractions.Game;

public interface IQuestService
{
    Task<QuestDTO> Get(int questId);
    
    Task<int> Create(int accessorUserId, QuestCreationDTO questCreationDto);

    Task Delete(int accessorUserId, int questId);
}