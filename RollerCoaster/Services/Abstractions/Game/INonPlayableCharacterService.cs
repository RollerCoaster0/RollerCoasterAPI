using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.DataTransferObjects.Game.Fetching;

namespace RollerCoaster.Services.Abstractions.Game;

public interface INonPlayableCharacterService
{
    Task<NonPlayableCharacterDTO> Get(int id);
    
    Task<int> Create(int accessorId, NonPlayableCharacterCreationDTO npcCreationDto);

    Task Delete(int accessorId, int id);
}