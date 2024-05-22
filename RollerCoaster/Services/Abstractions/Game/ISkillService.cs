using RollerCoaster.DataTransferObjects.Game.Skills;

namespace RollerCoaster.Services.Abstractions.Game;

public interface ISkillService
{
    Task<SkillDTO> Get(int id);
    
    Task<int> Create(int accessorUserId, SkillCreationDTO skillCreationDto);

    Task Delete(int accessorUserId, int id);
}