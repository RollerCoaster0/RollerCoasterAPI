using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Controllers.Game;

[Route("skills")]
[ApiController]
public class SkillsController(ISkillService skillService): ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromQuery] SkillCreationDTO skillCreationDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var createdSkillId = await skillService.Create(int.Parse(userId), skillCreationDto);
        return Created("", new
        {
            Id = createdSkillId
        });
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await skillService.Delete(int.Parse(userId), id);
        return Ok();
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var skillDto = await skillService.Get(id);
        return Ok(skillDto);
    }
}