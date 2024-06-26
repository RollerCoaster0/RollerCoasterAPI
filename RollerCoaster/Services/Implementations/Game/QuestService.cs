using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game.Quests;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Services.Implementations.Game;

public class QuestService(DataBaseContext dataBaseContext): IQuestService
{
    public async Task<QuestDTO> Get(int questId)
    {
        var quest = await dataBaseContext.Quests.FindAsync(questId);

        if (quest is null)
            throw new NotFoundError("Квест не найден.");

        return new QuestDTO
        {
            Id = quest.Id,
            GameId = quest.GameId,
            Name = quest.Name,
            Description = quest.Description,
            HiddenDescription = quest.HiddenDescription
        };
    }

    public async Task<int> Create(int accessorUserId, QuestCreationDTO questCreationDto)
    {
        var game = await dataBaseContext.Games.FindAsync(questCreationDto.GameId);
        
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorUserId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");

        var quest = new Quest
        {
            GameId = questCreationDto.GameId,
            Name = questCreationDto.Name,
            Description = questCreationDto.Description,
            HiddenDescription = questCreationDto.HiddenDescription
        };

        await dataBaseContext.Quests.AddAsync(quest);
        await dataBaseContext.SaveChangesAsync();

        return quest.Id;
    }

    public async Task Delete(int accessorUserId, int questId)
    {
        var quest = await dataBaseContext.Quests.FindAsync(questId);
        if (quest is null)
            throw new NotFoundError("Квест не найден.");
        
        var game = await dataBaseContext.Games.FindAsync(quest.GameId);
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorUserId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");

        dataBaseContext.Quests.Remove(quest);
        await dataBaseContext.SaveChangesAsync();
    }
}