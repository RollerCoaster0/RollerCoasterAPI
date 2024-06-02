using Microsoft.AspNetCore.Mvc;

namespace RollerCoaster.DataTransferObjects.Game.NonPlayableCharacters;

public class NonPlayableCharacterAvatarLoadDTO
{
    [FromForm] public required IFormFile File { get; set; }
}