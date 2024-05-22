using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Common;
using RollerCoaster.DataTransferObjects.Game.Items;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Controllers.Game;

[Route("items")]
[ApiController]
[Authorize]
public class ItemsController(IItemService itemService): ControllerBase
{
    [HttpPost]
    [ProducesResponseType<IdOfCreatedObjectDTO>(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IdOfCreatedObjectDTO>> Create([FromQuery] ItemCreationDTO itemCreationDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var createdItemId = await itemService.Create(int.Parse(userId), itemCreationDto);
        return Created("", new IdOfCreatedObjectDTO { Id = createdItemId });
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await itemService.Delete(int.Parse(userId), id);
        return Ok();
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType<ItemDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ItemDTO>> Get(int id)
    {
        var itemDto = await itemService.Get(id);
        return Ok(itemDto);
    }
}