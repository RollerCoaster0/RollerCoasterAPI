using RollerCoaster.DataTransferObjects.Game.Items;

namespace RollerCoaster.Services.Abstractions.Game;

public interface IItemService
{
    Task<ItemDTO> Get(int id);
    
    Task<int> Create(int accessorUserId, ItemCreationDTO itemCreationDto);

    Task Delete(int accessorUserId, int id);
}