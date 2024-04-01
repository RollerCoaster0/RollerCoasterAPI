using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.DataTransferObjects.Game.Fetching;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Services.Realisations.Game;

public class SkillService(DataBaseContext dataBaseContext): ISkillService
{
    public async Task<SkillDTO> Get(int id)
    {
        var skill = await dataBaseContext.Skills.FindAsync(id);

        if (skill is null)
            throw new NotFoundError("Скилл не найден.");

        return new SkillDTO
        {
            Id = skill.Id,
            GameId = skill.GameId,
            Name = skill.Name,
            Description = skill.Description,
            AvailableForCharacterClassId = skill.AvailableForCharacterClassId
        };
    }

    public async Task<int> Create(int accessorId, SkillCreationDTO skillCreationDto)
    {
        var game = await dataBaseContext.Games.FindAsync(skillCreationDto.GameId);
        
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorId != accessorId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");

        var skill = new Skill
        {
            GameId = skillCreationDto.GameId,
            Name = skillCreationDto.Name,
            Description = skillCreationDto.Description,
            AvailableForCharacterClassId = skillCreationDto.AvailableForCharacterClassId
        };

        await dataBaseContext.Skills.AddAsync(skill);
        await dataBaseContext.SaveChangesAsync();

        return skill.Id;
    }

    public async Task Delete(int accessorId, int id)
    {
        var skill = await dataBaseContext.Skills.FindAsync(id);
        if (skill is null)
            throw new NotFoundError("Скилл не найден.");
        
        var game = await dataBaseContext.Games.FindAsync(skill.GameId);
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorId != accessorId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");

        dataBaseContext.Skills.Remove(skill);
        await dataBaseContext.SaveChangesAsync();
    }
}