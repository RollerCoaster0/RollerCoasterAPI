using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Controllers.Game;

[Route("npc")]
[ApiController]
public class NonPlayableCharacterController(INonPlayableCharacterService nonPlayableCharacterService): ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromQuery] NonPlayableCharacterCreationDTO nonPlayableCharacterCreationDto) 
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var createdNpcId = await nonPlayableCharacterService.Create(int.Parse(userId), nonPlayableCharacterCreationDto);
        return Created("", new
        {
            Id = createdNpcId
        });
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await nonPlayableCharacterService.Delete(int.Parse(userId), id);
        return Ok();
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var npcDto = await nonPlayableCharacterService.Get(id);
        return Ok(npcDto);
    }
}