using Microsoft.AspNetCore.Mvc;

namespace RollerCoaster.DataTransferObjects.Game.NonPlayableCharacters;

public class NonPlayableCharacterAvatarLoadDTO
{
    [FromRoute] public required int NonPlayableCharacterId { get; set; }
    [FromForm] public required IFormFile File { get; set; }
}