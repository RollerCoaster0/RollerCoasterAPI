using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.DataTransferObjects.Game.Fetching;

namespace RollerCoaster.Services.Abstractions.Game;

public interface INonPlayableCharacterService
{
    Task<NonPlayableCharacterDTO> Get(int id);
    
    Task<int> Create(int accessorUserId, NonPlayableCharacterCreationDTO npcCreationDto);
    
    Task LoadAvatar(int accessorUserId, NonPlayableCharacterAvatarLoadDTO nonPlayableCharacterAvatarLoadDto);

    Task Delete(int accessorUserId, int id);
}