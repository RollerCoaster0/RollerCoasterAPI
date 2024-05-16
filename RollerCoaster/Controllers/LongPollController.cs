using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Updates;
using RollerCoaster.DataTransferObjects.Users;
using RollerCoaster.LongPoll;

namespace RollerCoaster.Controllers.Session;

[Route("longpoll")]
[ApiController]
[Authorize]
public class LongPollController(ILongPollService longPollService): ControllerBase
{
    [HttpGet]
    [ProducesResponseType<LongPollUpdate>(StatusCodes.Status200OK)]
    public async Task<ActionResult<LongPollUpdate>> Poll()
    {
        // TODO: add timeout support 
        
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var update = await longPollService.DequeueUpdateAsync(int.Parse(userId));
    
        return Ok(update);
    }
}