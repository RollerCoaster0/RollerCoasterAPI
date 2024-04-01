using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.DataTransferObjects.Game.Fetching;

namespace RollerCoaster.Services.Abstractions.Game;

public interface ICharacterClassService
{
    Task<CharacterClassDTO> Get(int id);
    
    Task<int> Create(int accessorId, CharacterClassCreationDTO characterClassCreationDto);

    Task Delete(int accessorId, int id);
}