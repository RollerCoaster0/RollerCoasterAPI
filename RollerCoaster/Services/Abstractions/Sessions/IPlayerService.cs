using RollerCoaster.DataTransferObjects.Game.NonPlayableCharacters;
using RollerCoaster.DataTransferObjects.Session.Common;
using RollerCoaster.DataTransferObjects.Session.Players;
using RollerCoaster.DataTransferObjects.Session.Skills;

namespace RollerCoaster.Services.Abstractions.Sessions;

public interface IPlayerService
{
    Task<PlayerDTO> Get(int accessorUserId, int playerId);
    
    Task<List<PlayerDTO>> GetBySession(int accessorUserId, int sessionId);
    
    Task<int> Create(int accessorUserId, PlayerCreationDTO playerCreationDto);
    
    Task LoadAvatar(int accessorUserId, int id, PlayerAvatarLoadDTO playerAvatarLoadDto);
    
    Task Move(int accessorUserId, int playerId, MoveSomeoneDTO moveSomeoneDto);
    
    Task ChangeHealthPoints(int accessorUserId, int playerId, ChangeHealthPointsDTO changeHealthPointsDto);

    Task UseSkill(int accessorUserId, int playerId, UseSkillDTO useSkillDto);
    
    Task<RollResultDTO> Roll(int accessorUserId, int playerId, RollDTO rollDto);
}