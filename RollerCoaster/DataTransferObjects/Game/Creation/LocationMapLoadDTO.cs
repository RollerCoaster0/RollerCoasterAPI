using Microsoft.AspNetCore.Mvc;

namespace RollerCoaster.DataTransferObjects.Game.Creation;

public class LocationMapLoadDTO
{
    [FromRoute] public required int LocationId { get; set; }
    [FromForm] public required IFormFile File { get; set; }
}