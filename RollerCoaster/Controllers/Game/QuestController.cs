using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Controllers.Game;

[Route("[controller]")]
[ApiController]
public class QuestController(IQuestService questService): ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody, FromQuery] QuestCreationDTO questCreationDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var createdQuestId = await questService.Create(int.Parse(userId), questCreationDto);
        return Created("", new
        {
            Id = createdQuestId
        });
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await questService.Delete(int.Parse(userId), id);
        return Ok();
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var questDto = await questService.Get(id);
        return Ok(questDto);
    }
}