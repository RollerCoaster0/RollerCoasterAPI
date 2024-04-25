using Microsoft.AspNetCore.Mvc;

namespace RollerCoaster.DataTransferObjects.Game.Creation;

public class LocationMapLoadDTO
{
    [FromRoute] public required int LocationId { get; set; }
    [FromQuery] public required string Sizes { get; set; } // "300x200"
    [FromForm] public required IFormFile File { get; set; }
}