using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Common;
using RollerCoaster.DataTransferObjects.Session.Creation;
using RollerCoaster.DataTransferObjects.Session.Fetching;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Controllers.Session;

[Route("sessions")]
[ApiController]
[Authorize]
public class SessionsController(ISessionService sessionService): ControllerBase
{
    [HttpPost]
    [ProducesResponseType<IdOfCreatedObjectDTO>(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdOfCreatedObjectDTO>> Create([FromQuery] SessionCreationDTO sessionCreationDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var createdPlayerId = await sessionService.Create(int.Parse(userId), sessionCreationDto);
        return Created("", new IdOfCreatedObjectDTO { Id = createdPlayerId });
    }
    
    [HttpPost("{id:int}/start")]
    [ProducesResponseType<IdOfCreatedObjectDTO>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IdOfCreatedObjectDTO>> Start(int id)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await sessionService.Start(int.Parse(userId), id);
        return Ok();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await sessionService.Delete(int.Parse(userId), id);
        return Ok();
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SessionDTO>> Get(int id)
    {
        var gameDto = await sessionService.Get(id);
        return Ok(gameDto);
    }
}