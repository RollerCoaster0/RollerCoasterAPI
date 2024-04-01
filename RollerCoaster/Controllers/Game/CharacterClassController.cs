using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.DataTransferObjects.Game.Fetching;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Controllers.Game;

[Route("[controller]")]
[ApiController]
public class CharacterClassController(ICharacterClassService characterClassService): ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CharacterClassCreationDTO characterClassCreationDto)
    {
        try
        {
            var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
            var createdNpcId = await characterClassService.Create(int.Parse(userId), characterClassCreationDto);
            return Created("", new
            {
                Id = createdNpcId
            });
        }
        catch (NotFoundError e)
        {
            return NotFound(new {e.Message});
        }
        catch (AccessDeniedError e)
        {
            return Forbid(e.Message);
        }
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
            await characterClassService.Delete(int.Parse(userId), id);
            return Ok();
        }
        catch (NotFoundError e)
        {
            return NotFound(new {e.Message});
        }
        catch (AccessDeniedError e)
        {
            return Forbid(e.Message);
        }
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var locationDto = await characterClassService.Get(id);
            return Ok(locationDto);
        }
        catch (NotFoundError e)
        {
            return NotFound(new { e.Message });
        }
    }
}