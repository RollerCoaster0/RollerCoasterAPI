using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Controllers.Game;

[Route("[controller]")]
[ApiController]
public class QuestController(IQuestService questService): ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(QuestCreationDTO questCreationDto)
    {
        try
        {
            var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
            var createdNpcId = await questService.Create(int.Parse(userId), questCreationDto);
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
            await questService.Delete(int.Parse(userId), id);
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
            var locationDto = await questService.Get(id);
            return Ok(locationDto);
        }
        catch (NotFoundError e)
        {
            return NotFound(new { e.Message });
        }
    }
}