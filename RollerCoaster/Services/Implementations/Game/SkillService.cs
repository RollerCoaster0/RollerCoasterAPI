using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game.Skills;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Services.Implementations.Game;

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
            AvailableOnlyForCharacterClassId = skill.AvailableOnlyForCharacterClassId,
            AvailableOnlyForNonPlayableCharacterId = skill.AvailableOnlyForNonPlayableCharacterId
        };
    }

    public async Task<int> Create(int accessorUserId, SkillCreationDTO skillCreationDto) 
    {
        var game = await dataBaseContext.Games.FindAsync(skillCreationDto.GameId);
        
        if (game is null)
            throw new NotFoundError("Игра не найдена.");

        if (skillCreationDto.AvailableOnlyForNonPlayableCharacterId is not null)
        {
            var npc = await dataBaseContext.NonPlayableCharacters.FindAsync(
                skillCreationDto.AvailableOnlyForNonPlayableCharacterId);

            if (npc is null)
                throw new NotFoundError("NPC не найден.");
            
            if (skillCreationDto.GameId != npc.GameId)
                throw new ProvidedDataIsInvalidError("Этот NPC не принадлежит этой игре.");
        }

        if (skillCreationDto.AvailableOnlyForCharacterClassId is not null)
        {
            var characterClass = await dataBaseContext.CharacterClasses.FindAsync(
                skillCreationDto.AvailableOnlyForCharacterClassId);

            if (characterClass is null)
                throw new NotFoundError("Класс персонажа не найден.");

            if (skillCreationDto.GameId != characterClass.GameId)
                throw new ProvidedDataIsInvalidError("Этот класс персонажа не принадлежит этой игре.");
        }
        
        if (game.CreatorUserId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");

        var skill = new Skill
        {
            GameId = skillCreationDto.GameId,
            Name = skillCreationDto.Name,
            Description = skillCreationDto.Description,
            AvailableOnlyForCharacterClassId = skillCreationDto.AvailableOnlyForCharacterClassId,
            AvailableOnlyForNonPlayableCharacterId = skillCreationDto.AvailableOnlyForNonPlayableCharacterId
        };

        await dataBaseContext.Skills.AddAsync(skill);
        await dataBaseContext.SaveChangesAsync();

        return skill.Id;
    }

    public async Task Delete(int accessorUserId, int id)
    {
        var skill = await dataBaseContext.Skills.FindAsync(id);
        if (skill is null)
            throw new NotFoundError("Скилл не найден.");
        
        var game = await dataBaseContext.Games.FindAsync(skill.GameId);
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorUserId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");

        dataBaseContext.Skills.Remove(skill);
        await dataBaseContext.SaveChangesAsync();
    }
}