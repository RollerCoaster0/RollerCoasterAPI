using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Common;
using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.DataTransferObjects.Game.Fetching;
using RollerCoaster.DataTransferObjects.Session;
using RollerCoaster.Services.Abstractions.Game;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Controllers.Game;

[Route("quests")]
[ApiController]
[Authorize]
public class QuestsController(
    IQuestService questService,
    IQuestStatusService questStatusService): ControllerBase
{
    [HttpPost]
    [ProducesResponseType<IdOfCreatedObjectDTO>(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IdOfCreatedObjectDTO>> Create([FromQuery] QuestCreationDTO questCreationDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var createdQuestId = await questService.Create(int.Parse(userId), questCreationDto);
        return Created("", new IdOfCreatedObjectDTO { Id = createdQuestId });
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await questService.Delete(int.Parse(userId), id);
        return Ok();
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType<QuestDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QuestDTO>> Get(int id)
    {
        var questDto = await questService.Get(id);
        return Ok(questDto);
    }
    
    // TODO: type hints
    
    [HttpPost("{id:int}/status")]
    public async Task<ActionResult> SetStatus(int id, [FromQuery] QuestChangeStatusDTO questChangeStatusDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await questStatusService.SetStatus(int.Parse(userId), id, questChangeStatusDto);
        return Ok();
    }
    
    [HttpGet("{id:int}/status")]
    public async Task<ActionResult<QuestStatusDTO>> GetStatus(int id, [FromQuery] QuestFetchStatusDTO questFetchStatusDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var questStatusDto = await questStatusService.GetStatus(int.Parse(userId), id, questFetchStatusDto);
        return Ok(questStatusDto);
    }
}