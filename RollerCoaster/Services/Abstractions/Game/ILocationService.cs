using RollerCoaster.DataTransferObjects.Game.Locations;

namespace RollerCoaster.Services.Abstractions.Game;

public interface ILocationService
{
    Task<LocationDTO> Get(int id);
    
    Task<int> Create(int accessorUserId, LocationCreationDTO locationCreationDto);
    
    Task<LoadedMapDTO> LoadMap(int accessorUserId, int id, LocationMapLoadDTO locationMapLoadDto);

    Task Delete(int accessorUserId, int id);
}