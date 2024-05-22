using RollerCoaster.DataBase;
using RollerCoaster.DataTransferObjects;
using RollerCoaster.DataTransferObjects.Session;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Services.Realisations.Session;

// тут все доступно только ГМу
public class ActiveNonPlayableCharactersService(
    DataBaseContext dataBaseContext, 
    IRollService rollService): IActiveNonPlayableCharactersService
{
    public async Task Move(int accessorUserId, int anpcId, MoveSomeoneDTO moveSomeoneDto)
    {
        var anpc = await dataBaseContext.ActiveNonPlayableCharacters.FindAsync(anpcId);
        if (anpc is null)
            throw new NotFoundError("NPC не найден.");

        var session = await dataBaseContext.Sessions.FindAsync(anpc.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найден.");
        
        if (session.GameMasterId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этому.");
        
        // TODO: validate coordinates 
        anpc.CurrentXPosition = moveSomeoneDto.X;
        anpc.CurrentYPosition = moveSomeoneDto.Y;

        await dataBaseContext.SaveChangesAsync();
    }

    public async Task ChangeHealthPoints(int accessorUserId, int anpcId, ChangeHealthPointsDTO changeHealthPointsDto)
    {
        var anpc = await dataBaseContext.ActiveNonPlayableCharacters.FindAsync(anpcId);
        if (anpc is null)
            throw new NotFoundError("NPC не найден.");

        var session = await dataBaseContext.Sessions.FindAsync(anpc.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найден.");
        
        if (session.GameMasterId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этому.");

        anpc.HealthPoints = changeHealthPointsDto.HP;
        
        await dataBaseContext.SaveChangesAsync();
    }

    public async Task UseSkill(int accessorUserId, int anpcId, UseSkillDTO useSkillDto)
    {
        var anpc = await dataBaseContext.ActiveNonPlayableCharacters.FindAsync(anpcId);
        if (anpc is null)
            throw new NotFoundError("NPC не найден.");

        var session = await dataBaseContext.Sessions.FindAsync(anpc.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найден.");
        
        if (session.GameMasterId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этому.");
        
        var skill = await dataBaseContext.Skills.FindAsync(useSkillDto.SkillId);
        
        if (skill is null)
            throw new NotFoundError("Скилл не найден.");
    }
    
    public async Task<RollResultDTO> Roll(int accessorUserId, int anpcId, RollDTO rollDto)
    {
        var anpc = await dataBaseContext.ActiveNonPlayableCharacters.FindAsync(anpcId);
        if (anpc is null)
            throw new NotFoundError("NPC не найден.");

        var session = await dataBaseContext.Sessions.FindAsync(anpc.SessionId);
        if (session is null)
            throw new NotFoundError("Сессия не найден.");
        
        if (session.GameMasterId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этому.");
        
        var rollResult = await rollService.Roll(rollDto.Die);
        
        return new RollResultDTO
        {
            Result = rollResult
        };
    }
}