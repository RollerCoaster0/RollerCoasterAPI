using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.DataTransferObjects.Game.Fetching;

namespace RollerCoaster.Services.Abstractions.Game;

public interface ILocationService
{
    Task<LocationDTO> Get(int id);
    
    Task<int> Create(int accessorUserId, LocationCreationDTO locationCreationDto);
    
    Task LoadMap(int accessorUserId, LocationMapLoadDTO locationMapLoadDto);

    Task Delete(int accessorUserId, int id);
}