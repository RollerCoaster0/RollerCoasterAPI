using Microsoft.AspNetCore.Mvc;

namespace RollerCoaster.DataTransferObjects.Game.Creation;

public class NonPlayableCharacterAvatarLoadDTO
{
    [FromRoute] public required int NonPlayableCharacterId { get; set; }
    [FromForm] public required IFormFile File { get; set; }
}