using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.DataTransferObjects.Game.Fetching;

namespace RollerCoaster.Services.Abstractions.Game;

public interface ICharacterClassService
{
    Task<CharacterClassDTO> Get(int id);
    
    Task<int> Create(int accessorUserId, CharacterClassCreationDTO characterClassCreationDto);

    Task Delete(int accessorUserId, int id);
}