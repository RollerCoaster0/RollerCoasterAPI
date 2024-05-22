using RollerCoaster.DataBase;
using RollerCoaster.DataTransferObjects;
using RollerCoaster.DataTransferObjects.Session;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Services.Realisations.Session;

public class ActiveNonPlayableCharactersService(
    DataBaseContext dataBaseContext, 
    IRollService rollService): IActiveNonPlayableCharactersService
{
    public Task Move(int accessorUserId, int anpcId, MoveSomeoneDTO moveSomeoneDto)
    {
        throw new NotImplementedException();
    }

    public Task ChangeHealthPoints(int accessorUserId, int anpcId, ChangeHealthPointsDTO changeHealthPointsDto)
    {
        throw new NotImplementedException();
    }

    public Task UseSkill(int accessorUserId, int anpcId, UseSkillDTO useSkillDto)
    {
        throw new NotImplementedException();
    }

    public Task<RollResultDTO> Roll(int accessorUserId, int anpcId, RollDTO rollDto)
    {
        throw new NotImplementedException();
    }
}