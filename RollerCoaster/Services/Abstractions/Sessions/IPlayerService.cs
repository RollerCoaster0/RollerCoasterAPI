using RollerCoaster.DataTransferObjects;
using RollerCoaster.DataTransferObjects.Session;
using RollerCoaster.DataTransferObjects.Session.Creation;
using RollerCoaster.DataTransferObjects.Session.Fetching;

namespace RollerCoaster.Services.Abstractions.Sessions;

public interface IPlayerService
{
    public Task<PlayerDTO> Get(int accessorUserId, int playerId);
    
    public Task<int> Create(int accessorUserId, PlayerCreationDTO playerCreationDto);
    
    Task Move(int accessorId, int playerId, MoveSomeoneDTO moveSomeoneDto);
    
    Task ChangeHealthPoints(int accessorId, int playerId, ChangeHealthPointsDTO changeHealthPointsDto);

    Task UseSkill(int accessorId, int playerId, UseSkillDTO useSkillDto);
    
    Task<RollResultDTO> Roll(int accessorId, int playerId, RollDTO rollDto);
}