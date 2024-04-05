using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.DataTransferObjects.Game.Fetching;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Services.Realisations.Game;

public class NonPlayableCharacterService(DataBaseContext dataBaseContext): INonPlayableCharacterService
{
    public async Task<NonPlayableCharacterDTO> Get(int id)
    {
        var npc = await dataBaseContext.NonPlayableCharacters.FindAsync(id);

        if (npc is null)
            throw new NotFoundError("NPC не найден.");

        return new NonPlayableCharacterDTO
        {
            Id = npc.Id,
            GameId = npc.GameId,
            Name = npc.Name,
            BaseLocationId = npc.BaseLocationId
        };
    }

    public async Task<int> Create(int accessorId, NonPlayableCharacterCreationDTO npcCreationDto)
    {
        // TODO: Validation for BaseLocationId
        var game = await dataBaseContext.Games.FindAsync(npcCreationDto.GameId);
        
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorId != accessorId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");

        var npc = new NonPlayableCharacter
        {
            GameId = npcCreationDto.GameId,
            Name = npcCreationDto.Name,
            BaseLocationId = npcCreationDto.BaseLocationId
        };

        await dataBaseContext.NonPlayableCharacters.AddAsync(npc);
        await dataBaseContext.SaveChangesAsync();

        return npc.Id;
    }

    public async Task Delete(int accessorId, int id)
    {
        var npc = await dataBaseContext.NonPlayableCharacters.FindAsync(id);
        if (npc is null)
            throw new NotFoundError("NPC не найден.");
        
        var game = await dataBaseContext.Games.FindAsync(npc.GameId);
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorId != accessorId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");

        dataBaseContext.NonPlayableCharacters.Remove(npc);
        await dataBaseContext.SaveChangesAsync();
    }
}