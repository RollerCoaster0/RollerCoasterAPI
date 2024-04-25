using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.DataTransferObjects.Game.Fetching;

namespace RollerCoaster.Services.Abstractions.Game;

public interface INonPlayableCharacterService
{
    Task<NonPlayableCharacterDTO> Get(int id);
    
    Task<int> Create(int accessorId, NonPlayableCharacterCreationDTO npcCreationDto);
    
    Task LoadAvatar(int accessorId, NonPlayableCharacterAvatarLoadDTO nonPlayableCharacterAvatarLoadDto);

    Task Delete(int accessorId, int id);
}