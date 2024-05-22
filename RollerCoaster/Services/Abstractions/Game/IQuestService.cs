using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.DataTransferObjects.Game.Fetching;
namespace RollerCoaster.Services.Abstractions.Game;

public interface IQuestService
{
    Task<QuestDTO> Get(int questId);
    
    Task<int> Create(int accessorUserId, QuestCreationDTO questCreationDto);

    Task Delete(int accessorUserId, int questId);
}