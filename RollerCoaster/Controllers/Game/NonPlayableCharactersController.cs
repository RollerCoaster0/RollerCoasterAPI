using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Common;
using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.DataTransferObjects.Game.Fetching;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Controllers.Game;

[Route("npc")]
[ApiController]
public class NonPlayableCharactersController(INonPlayableCharacterService nonPlayableCharacterService): ControllerBase
{
    [HttpPost]
    [ProducesResponseType<IdOfCreatedObjectDTO>(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IdOfCreatedObjectDTO>> Create([FromQuery] NonPlayableCharacterCreationDTO nonPlayableCharacterCreationDto) 
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var createdNpcId = await nonPlayableCharacterService.Create(int.Parse(userId), nonPlayableCharacterCreationDto);
        return Created("", new IdOfCreatedObjectDTO { Id = createdNpcId });
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await nonPlayableCharacterService.Delete(int.Parse(userId), id);
        return Ok();
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType<NonPlayableCharacterDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NonPlayableCharacterDTO>> Get(int id)
    {
        var npcDto = await nonPlayableCharacterService.Get(id);
        return Ok(npcDto);
    }
}