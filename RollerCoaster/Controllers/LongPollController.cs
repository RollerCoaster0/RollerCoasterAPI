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
    [ProducesResponseType<LongPollResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<LongPollResponse>> Poll([FromQuery] string? deviceId)
    {
        // TODO: add timeout support 
        
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var update = await longPollService.DequeueUpdateAsync(int.Parse(userId), deviceId);
    
        return Ok(update);
    }
}