using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Common;
using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.DataTransferObjects.Game.Fetching;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Controllers.Game;

[Route("locations")]
[ApiController]
[Authorize]
public class LocationsController(
    ILocationService locationService): ControllerBase
{
    [HttpPost]
    [ProducesResponseType<IdOfCreatedObjectDTO>(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromQuery] LocationCreationDTO locationCreationDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var createdLocationId = await locationService.Create(int.Parse(userId), locationCreationDto);
        return Created("", new IdOfCreatedObjectDTO { Id = createdLocationId });
    }
    
    [HttpPost("{locationId:int}/map")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> LoadMap(LocationMapLoadDTO locationMapLoadDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await locationService.LoadMap(int.Parse(userId), locationMapLoadDto); 
        return Ok();
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Delete(int id)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await locationService.Delete(int.Parse(userId), id);
        return Ok();
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType<LocationDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LocationDTO>> Get(int id)
    {
        var locationDto = await locationService.Get(id);
        return Ok(locationDto);
    }
}