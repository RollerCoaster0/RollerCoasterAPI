using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Controllers.Game;

[Route("[controller]")]
[ApiController]
public class CharacterClassController(ICharacterClassService characterClassService): ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromQuery] CharacterClassCreationDTO characterClassCreationDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var createdCharacterClassId = await characterClassService.Create(int.Parse(userId), characterClassCreationDto);
        return Created("", new
        {
            Id = createdCharacterClassId
        });
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await characterClassService.Delete(int.Parse(userId), id);
        return Ok();
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var characterClassDto = await characterClassService.Get(id);
        return Ok(characterClassDto);
    }
}