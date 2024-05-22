using RollerCoaster.DataTransferObjects;
using RollerCoaster.DataTransferObjects.Session;
using RollerCoaster.DataTransferObjects.Session.Creation;
using RollerCoaster.DataTransferObjects.Session.Fetching;

namespace RollerCoaster.Services.Abstractions.Sessions;

public interface IPlayerService
{
    public Task<PlayerDTO> Get(int accessorUserId, int playerId);
    
    public Task<int> Create(int accessorUserId, PlayerCreationDTO playerCreationDto);
    
    Task Move(int accessorUserId, int playerId, MoveSomeoneDTO moveSomeoneDto);
    
    Task ChangeHealthPoints(int accessorUserId, int playerId, ChangeHealthPointsDTO changeHealthPointsDto);

    Task UseSkill(int accessorUserId, int playerId, UseSkillDTO useSkillDto);
    
    Task<RollResultDTO> Roll(int accessorUserId, int playerId, RollDTO rollDto);
}