using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Common;
using RollerCoaster.DataTransferObjects.Session;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Controllers.Session;

[Route("sessions")]
[ApiController]
[Authorize]
public class SessionsController(ISessionService sessionService): ControllerBase
{
    [HttpPost]
    [ProducesResponseType<IdOfCreatedObjectDTO>(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IdOfCreatedObjectDTO>> Create([FromQuery] SessionCreationDTO sessionCreationDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var createdPlayerId = await sessionService.Create(int.Parse(userId), sessionCreationDto);
        return Created("", new IdOfCreatedObjectDTO { Id = createdPlayerId });
    }
    
    [HttpPost("{id:int}/start")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Start(int id)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await sessionService.Start(int.Parse(userId), id);
        return Ok();
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await sessionService.Delete(int.Parse(userId), id);
        return Ok();
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType<SessionDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SessionDTO>> Get(int id)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var sessionDto = await sessionService.Get(int.Parse(userId), id);
        return Ok(sessionDto);
    }
    
    [HttpPost("{id:int}/changeLocation")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ChangeLocation(int id, [FromQuery] ChangeLocationDTO locationDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await sessionService.ChangeLocation(int.Parse(userId), id, locationDto);
        return Ok();
    }
}