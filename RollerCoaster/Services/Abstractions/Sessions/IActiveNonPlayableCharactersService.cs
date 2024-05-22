using RollerCoaster.DataTransferObjects;
using RollerCoaster.DataTransferObjects.Session;

namespace RollerCoaster.Services.Abstractions.Sessions;

public interface IActiveNonPlayableCharactersService
{
    Task Move(int accessorId, int anpcId, MoveSomeoneDTO moveSomeoneDto);
    
    Task ChangeHealthPoints(int accessorId, int anpcId, ChangeHealthPointsDTO changeHealthPointsDto);

    Task UseSkill(int accessorId, int anpcId, UseSkillDTO useSkillDto);

    Task<RollResultDTO> Roll(int accessorId, int anpcId, RollDTO rollDto);
}