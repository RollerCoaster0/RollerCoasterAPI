using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects;
using RollerCoaster.DataTransferObjects.Session;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Controllers.Session;

[Route("anpc")]
[ApiController]
[Authorize]
public class ActiveNonPlayableCharactersController(
    IActiveNonPlayableCharactersService activeNonPlayableCharactersService): ControllerBase
{
    // TODO: type hints
    
    [HttpPost("{id:int}/move")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Move(int id, [FromQuery] MoveSomeoneDTO moveSomeoneDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await activeNonPlayableCharactersService.Move(int.Parse(userId), id, moveSomeoneDto);
        return Ok();
    }
    
    [HttpPost("{id:int}/changeHp")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> ChangeHp(int id, [FromQuery] ChangeHealthPointsDTO changeHealthPointsDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await activeNonPlayableCharactersService.ChangeHealthPoints(int.Parse(userId), id, changeHealthPointsDto);
        return Ok();
    }
    
    [HttpPost("{id:int}/useSkill")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> UseSKill(int id, [FromQuery] UseSkillDTO useSkillDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await activeNonPlayableCharactersService.UseSkill(int.Parse(userId), id, useSkillDto);
        return Ok();
    }
    
    [HttpPost("{id:int}/roll")]
    [ProducesResponseType<RollResultDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<RollResultDTO>> ROll(int id, [FromQuery] RollDTO rollDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var result = await activeNonPlayableCharactersService.Roll(int.Parse(userId), id, rollDto);
        return Ok(result);
    }
}