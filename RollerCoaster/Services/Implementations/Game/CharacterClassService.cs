using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game.CharacterClasses;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Services.Implementations.Game;

public class CharacterClassService(DataBaseContext dataBaseContext): ICharacterClassService
{
    public async Task<CharacterClassDTO> Get(int id)
    {
        var item = await dataBaseContext.CharacterClasses.FindAsync(id);

        if (item is null)
            throw new NotFoundError("Класс не найден.");

        return new CharacterClassDTO
        {
            Id = item.Id,
            GameId = item.GameId,
            Name = item.Name,
            Description = item.Description
        };
    }

    public async Task<int> Create(int accessorUserId, CharacterClassCreationDTO characterClassCreationDto)
    {
        var game = await dataBaseContext.Games.FindAsync(characterClassCreationDto.GameId);
        
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");

        var characterClass = new CharacterClass
        {
            GameId = characterClassCreationDto.GameId,
            Name = characterClassCreationDto.Name,
            Description = characterClassCreationDto.Description,
        };

        await dataBaseContext.CharacterClasses.AddAsync(characterClass);
        await dataBaseContext.SaveChangesAsync();

        return characterClass.Id;
    }

    public async Task Delete(int accessorUserId, int id)
    {
        var characterClass = await dataBaseContext.CharacterClasses.FindAsync(id);
        if (characterClass is null)
            throw new NotFoundError("Класс не найден.");
        
        var game = await dataBaseContext.Games.FindAsync(characterClass.GameId);
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");

        dataBaseContext.CharacterClasses.Remove(characterClass);
        await dataBaseContext.SaveChangesAsync();
    }
}