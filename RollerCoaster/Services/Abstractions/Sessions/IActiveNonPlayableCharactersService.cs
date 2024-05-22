using RollerCoaster.DataTransferObjects;
using RollerCoaster.DataTransferObjects.Game.Skills;
using RollerCoaster.DataTransferObjects.Session.ActiveNonPlayableCharacter;
using RollerCoaster.DataTransferObjects.Session.Common;

namespace RollerCoaster.Services.Abstractions.Sessions;

public interface IActiveNonPlayableCharactersService
{
    Task<ActiveNonPlayableCharacterDTO> Get(int accessorUserId, int anpcId);
    
    Task Move(int accessorUserId, int anpcId, MoveSomeoneDTO moveSomeoneDto);
    
    Task ChangeHealthPoints(int accessorUserId, int anpcId, ChangeHealthPointsDTO changeHealthPointsDto);

    Task UseSkill(int accessorUserId, int anpcId, UseSkillDTO useSkillDto);

    Task<RollResultDTO> Roll(int accessorUserId, int anpcId, RollDTO rollDto);
}