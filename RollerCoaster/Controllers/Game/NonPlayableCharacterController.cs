using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.DataTransferObjects.Game.Fetching;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Game;
using RollerCoaster.Services.Realisations.Game;

namespace RollerCoaster.Controllers.Game;

[Route("npc")]
[ApiController]
public class NonPlayableCharacterController(INonPlayableCharacterService nonPlayableCharacterService): ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(NonPlayableCharacterCreationDTO nonPlayableCharacterCreationDto) 
    {
        try
        {
            var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
            var createdNpcId = await nonPlayableCharacterService.Create(int.Parse(userId), nonPlayableCharacterCreationDto);
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
            await nonPlayableCharacterService.Delete(int.Parse(userId), id);
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
            var npcDto = await nonPlayableCharacterService.Get(id);
            return Ok(npcDto);
        }
        catch (NotFoundError e)
        {
            return NotFound(new { e.Message });
        }
    }
}