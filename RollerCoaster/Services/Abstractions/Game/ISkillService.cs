using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.DataTransferObjects.Game.Fetching;

namespace RollerCoaster.Services.Abstractions.Game;

public interface ISkillService
{
    Task<SkillDTO> Get(int id);
    
    Task<int> Create(int accessorId, SkillCreationDTO skillCreationDto);

    Task Delete(int accessorId, int id);
}