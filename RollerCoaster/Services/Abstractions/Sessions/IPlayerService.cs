using RollerCoaster.DataTransferObjects;
using RollerCoaster.DataTransferObjects.Game.Skills;
using RollerCoaster.DataTransferObjects.Session.Common;
using RollerCoaster.DataTransferObjects.Session.Players;

namespace RollerCoaster.Services.Abstractions.Sessions;

public interface IPlayerService
{
    Task<PlayerDTO> Get(int accessorUserId, int playerId);
    
    Task<int> Create(int accessorUserId, PlayerCreationDTO playerCreationDto);
    
    Task Move(int accessorUserId, int playerId, MoveSomeoneDTO moveSomeoneDto);
    
    Task ChangeHealthPoints(int accessorUserId, int playerId, ChangeHealthPointsDTO changeHealthPointsDto);

    Task UseSkill(int accessorUserId, int playerId, UseSkillDTO useSkillDto);
    
    Task<RollResultDTO> Roll(int accessorUserId, int playerId, RollDTO rollDto);
}