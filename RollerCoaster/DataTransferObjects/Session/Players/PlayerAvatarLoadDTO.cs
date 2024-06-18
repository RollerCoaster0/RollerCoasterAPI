using Microsoft.AspNetCore.Mvc;

namespace RollerCoaster.DataTransferObjects.Game.NonPlayableCharacters;

public class PlayerAvatarLoadDTO
{
    [FromForm] public required IFormFile File { get; set; }
}