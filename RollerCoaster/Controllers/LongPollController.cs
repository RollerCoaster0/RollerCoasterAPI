using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.LongPoll;
using RollerCoaster.Services.Abstractions.LongPoll;

namespace RollerCoaster.Controllers;

[Route("longpoll")]
[ApiController]
[Authorize]
public class LongPollController(ILongPollService longPollService): ControllerBase
{
    // TODO: type hints
    
    [HttpGet]
    [ProducesResponseType<LongPollUpdate>(StatusCodes.Status200OK)]
    public async Task<ActionResult<LongPollUpdate>> Poll()
    {
        // TODO: add timeout support 
        // TODO: push events to queue
        
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var update = await longPollService.DequeueUpdateAsync(int.Parse(userId));
    
        return Ok(update);
    }
}