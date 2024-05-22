using RollerCoaster.DataTransferObjects;
using RollerCoaster.DataTransferObjects.Session;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Services.Realisations.Session;

public class ActiveNonPlayableCharactersService: IActiveNonPlayableCharactersService
{
    public Task Move(int accessorId, int anpcId, MoveSomeoneDTO moveSomeoneDto)
    {
        throw new NotImplementedException();
    }

    public Task ChangeHealthPoints(int accessorId, int anpcId, ChangeHealthPointsDTO changeHealthPointsDto)
    {
        throw new NotImplementedException();
    }

    public Task UseSkill(int accessorId, int anpcId, UseSkillDTO useSkillDto)
    {
        throw new NotImplementedException();
    }

    public Task<RollResultDTO> Roll(int accessorId, int anpcId, RollDTO rollDto)
    {
        throw new NotImplementedException();
    }
}