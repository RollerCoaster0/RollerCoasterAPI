using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game.Items;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Services.Implementations.Game;

public class ItemService(DataBaseContext dataBaseContext): IItemService
{
    public async Task<ItemDTO> Get(int id)
    {
        var item = await dataBaseContext.Items.FindAsync(id);

        if (item is null)
            throw new NotFoundError("Предмет не найден.");

        return new ItemDTO
        {
            Id = item.Id,
            GameId = item.GameId,
            Name = item.Name,
            Description = item.Description,
            ItemType = item.ItemType
        };
    }

    public async Task<int> Create(int accessorUserId, ItemCreationDTO itemCreationDto)
    {
        var game = await dataBaseContext.Games.FindAsync(itemCreationDto.GameId);
        
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");

        var item = new Item
        {
            GameId = itemCreationDto.GameId,
            Name = itemCreationDto.Name,
            Description = itemCreationDto.Description,
            ItemType = itemCreationDto.ItemType // TODO: validate
        };

        await dataBaseContext.Items.AddAsync(item);
        await dataBaseContext.SaveChangesAsync();

        return item.Id;
    }

    public async Task Delete(int accessorUserId, int id)
    {
        var item = await dataBaseContext.Items.FindAsync(id);
        if (item is null)
            throw new NotFoundError("Предмет не найден.");
        
        var game = await dataBaseContext.Games.FindAsync(item.GameId);
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");

        dataBaseContext.Items.Remove(item);
        await dataBaseContext.SaveChangesAsync();
    }
}