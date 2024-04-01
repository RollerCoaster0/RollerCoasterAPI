using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.DataTransferObjects.Game.Fetching;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Services.Realisations.Game;

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
            ItemType = item.ItemType.ToString()
        };
    }

    public async Task<int> Create(int accessorId, ItemCreationDTO itemCreationDto)
    {
        if (itemCreationDto.Name.Length > 50)
            throw new ProvidedDataIsInvalidError("Название слишком длинное");
        
        if (itemCreationDto.Description.Length > 512)
            throw new ProvidedDataIsInvalidError("Описание слишком длинное");
        
        var game = await dataBaseContext.Games.FindAsync(itemCreationDto.GameId);
        
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorId != accessorId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");

        var item = new Item
        {
            GameId = itemCreationDto.GameId,
            Name = itemCreationDto.Name,
            Description = itemCreationDto.Description,
            ItemType = ItemType.Armor // TODO: fix
        };

        await dataBaseContext.Items.AddAsync(item);
        await dataBaseContext.SaveChangesAsync();

        return item.Id;
    }

    public async Task Delete(int accessorId, int id)
    {
        var item = await dataBaseContext.Items.FindAsync(id);
        if (item is null)
            throw new NotFoundError("Предмет не найден.");
        
        var game = await dataBaseContext.Games.FindAsync(item.GameId);
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorId != accessorId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");

        dataBaseContext.Items.Remove(item);
        await dataBaseContext.SaveChangesAsync();
    }
}