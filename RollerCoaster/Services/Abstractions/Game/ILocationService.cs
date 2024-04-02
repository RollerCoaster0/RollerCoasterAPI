using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.DataTransferObjects.Game.Fetching;

namespace RollerCoaster.Services.Abstractions.Game;

public interface ILocationService
{
    Task<LocationDTO> Get(int id);
    
    Task<int> Create(int accessorId, LocationCreationDTO locationCreationDto);
    
    Task LoadMap(int accessorId, LocationMapLoadDTO locationMapLoadDto);

    Task Delete(int accessorId, int id);
}