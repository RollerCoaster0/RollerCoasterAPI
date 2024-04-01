using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.DataTransferObjects.Game.Fetching;

namespace RollerCoaster.Services.Abstractions.Game;

public interface IItemService
{
    Task<ItemDTO> Get(int id);
    
    Task<int> Create(int accessorId, ItemCreationDTO itemCreationDto);

    Task Delete(int accessorId, int id);
}