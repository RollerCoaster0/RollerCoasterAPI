using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Common;
using RollerCoaster.DataTransferObjects.Game.NonPlayableCharacters;
using RollerCoaster.DataTransferObjects.Session.Common;
using RollerCoaster.DataTransferObjects.Session.Players;
using RollerCoaster.DataTransferObjects.Session.Skills;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Controllers.Session;

[Route("players")]
[ApiController]
[Authorize]
public class PlayersController(IPlayerService playerService): ControllerBase
{
    [HttpPost]
    [ProducesResponseType<IdOfCreatedObjectDTO>(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IdOfCreatedObjectDTO>> Create([FromQuery] PlayerCreationDTO playerCreationDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var createdPlayerId = await playerService.Create(int.Parse(userId), playerCreationDto);
        return Created("", new IdOfCreatedObjectDTO { Id = createdPlayerId });
    }
    
    [HttpPost("{id:int}/avatar")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> LoadAvatar(int id, PlayerAvatarLoadDTO playerAvatarLoadDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await playerService.LoadAvatar(int.Parse(userId), id, playerAvatarLoadDto); 
        return Ok();
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType<PlayerDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlayerDTO>> Get(int id)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var playerDto = await playerService.Get(int.Parse(userId), id);
        return Ok(playerDto);
    }
    
    [HttpGet]
    [ProducesResponseType<List<PlayerDTO>>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<PlayerDTO>>> GetBySession([FromQuery] int sessionId)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var playersDto = await playerService.GetBySession(int.Parse(userId), sessionId);
        return Ok(playersDto);
    }
    
    [HttpPost("{id:int}/move")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
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
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
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
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
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
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RollResultDTO>> Roll(int id, [FromQuery] RollDTO rollDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var result = await playerService.Roll(int.Parse(userId), id, rollDto);
        return Ok(result);
    } 
}