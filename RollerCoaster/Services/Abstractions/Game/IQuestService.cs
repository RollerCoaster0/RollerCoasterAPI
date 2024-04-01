using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.DataTransferObjects.Game.Fetching;

namespace RollerCoaster.Services.Abstractions.Game;

public interface IQuestService
{
    Task<QuestDTO> Get(int id);
    
    Task<int> Create(int accessorId, QuestCreationDTO questCreationDto);

    Task Delete(int accessorId, int id);
}