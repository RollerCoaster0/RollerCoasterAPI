using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects;
using RollerCoaster.DataTransferObjects.Common;
using RollerCoaster.DataTransferObjects.Session;
using RollerCoaster.DataTransferObjects.Session.Creation;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Controllers.Session;

[Route("players")]
[ApiController]
[Authorize]
public class PlayersController(IPlayerService playerService): ControllerBase
{
    [HttpPost]
    [ProducesResponseType<IdOfCreatedObjectDTO>(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdOfCreatedObjectDTO>> Create([FromQuery] PlayerCreationDTO playerCreationDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var createdPlayerId = await playerService.Create(int.Parse(userId), playerCreationDto);
        return Created("", new IdOfCreatedObjectDTO { Id = createdPlayerId });
    }
    
    [HttpPost("{id:int}/move")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Move(int id, [FromQuery] MoveSomeoneDTO moveSomeoneDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await playerService.Move(int.Parse(userId), id, moveSomeoneDto);
        return Ok();
    }
    
    [HttpPost("{id:int}/changeHp")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> ChangeHp(int id, [FromQuery] ChangeHealthPointsDTO changeHealthPointsDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await playerService.ChangeHealthPoints(int.Parse(userId), id, changeHealthPointsDto);
        return Ok();
    }
    
    [HttpPost("{id:int}/useSkill")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> UseSKill(int id, [FromQuery] UseSkillDTO useSkillDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await playerService.UseSkill(int.Parse(userId), id, useSkillDto);
        return Ok();
    }
    
    [HttpPost("{id:int}/roll")]
    [ProducesResponseType<RollResultDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<RollResultDTO>> ROll(int id, [FromQuery] RollDTO rollDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var result = await playerService.Roll(int.Parse(userId), id, rollDto);
        return Ok(result);
    } 
}