using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Common;
using RollerCoaster.DataTransferObjects.Game.NonPlayableCharacters;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Controllers.Game;

[Route("npc")]
[ApiController]
[Authorize]
public class NonPlayableCharactersController(
    INonPlayableCharacterService nonPlayableCharacterService): ControllerBase
{
    [HttpPost]
    [ProducesResponseType<IdOfCreatedObjectDTO>(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IdOfCreatedObjectDTO>> Create(
        [FromQuery] NonPlayableCharacterCreationDTO nonPlayableCharacterCreationDto) 
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var createdNpcId = await nonPlayableCharacterService.Create(
            int.Parse(userId), 
            nonPlayableCharacterCreationDto);
        return Created("", new IdOfCreatedObjectDTO { Id = createdNpcId });
    }
    
    [HttpPost("{id:int}/avatar")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> LoadMap(
        int id, NonPlayableCharacterAvatarLoadDTO nonPlayableCharacterAvatarLoadDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await nonPlayableCharacterService.LoadAvatar(int.Parse(userId), id, nonPlayableCharacterAvatarLoadDto); 
        return Ok();
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await nonPlayableCharacterService.Delete(int.Parse(userId), id);
        return Ok();
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType<NonPlayableCharacterDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NonPlayableCharacterDTO>> Get(int id)
    {
        var npcDto = await nonPlayableCharacterService.Get(id);
        return Ok(npcDto);
    }
}