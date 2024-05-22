using RollerCoaster.DataTransferObjects.Game.CharacterClasses;

namespace RollerCoaster.Services.Abstractions.Game;

public interface ICharacterClassService
{
    Task<CharacterClassDTO> Get(int id);
    
    Task<int> Create(int accessorUserId, CharacterClassCreationDTO characterClassCreationDto);

    Task Delete(int accessorUserId, int id);
}