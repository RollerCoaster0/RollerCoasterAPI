using Microsoft.AspNetCore.Mvc;

namespace RollerCoaster.DataTransferObjects.Game.Locations;

public class LocationMapLoadDTO
{
    [FromForm] public required IFormFile File { get; set; }
    [FromRoute] public required int LocationId { get; set; }
    [FromQuery] public required int Width { get; set; }
    [FromQuery] public required int Height { get; set; }
    [FromQuery] public required int BasePlayersXPosition { get; set; }
    [FromQuery] public required int BasePlayersYPosition { get; set; }
}