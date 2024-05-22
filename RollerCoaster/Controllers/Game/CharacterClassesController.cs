using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Common;
using RollerCoaster.DataTransferObjects.Game.CharacterClasses;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Controllers.Game;

[Route("classes")]
[ApiController]
[Authorize]
public class CharacterClassesController(ICharacterClassService characterClassService): ControllerBase
{
    [HttpPost]
    [ProducesResponseType<IdOfCreatedObjectDTO>(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IdOfCreatedObjectDTO>> Create(
        [FromQuery] CharacterClassCreationDTO characterClassCreationDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var createdCharacterClassId = await characterClassService.Create(
            int.Parse(userId),
            characterClassCreationDto);
        return Created("", new IdOfCreatedObjectDTO { Id = createdCharacterClassId });
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await characterClassService.Delete(int.Parse(userId), id);
        return Ok();
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType<CharacterClassDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CharacterClassDTO>> Get(int id)
    {
        var characterClassDto = await characterClassService.Get(id);
        return Ok(characterClassDto);
    }
}