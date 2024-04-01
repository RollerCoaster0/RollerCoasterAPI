using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Controllers.Game;

[Route("[controller]")]
[ApiController]
public class LocationController(ILocationService locationService): ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody, FromQuery] LocationCreationDTO locationCreationDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var createdLocationId = await locationService.Create(int.Parse(userId), locationCreationDto);
        return Created("", new
        {
            Id = createdLocationId
        });
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await locationService.Delete(int.Parse(userId), id);
        return Ok();
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var locationDto = await locationService.Get(id);
        return Ok(locationDto);
    }
}