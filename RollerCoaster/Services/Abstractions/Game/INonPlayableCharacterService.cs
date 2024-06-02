using RollerCoaster.DataTransferObjects.Game.NonPlayableCharacters;

namespace RollerCoaster.Services.Abstractions.Game;

public interface INonPlayableCharacterService
{
    Task<NonPlayableCharacterDTO> Get(int id);
    
    Task<int> Create(int accessorUserId, NonPlayableCharacterCreationDTO npcCreationDto);
    
    Task LoadAvatar(int accessorUserId, int id, NonPlayableCharacterAvatarLoadDTO nonPlayableCharacterAvatarLoadDto);

    Task Delete(int accessorUserId, int id);
}