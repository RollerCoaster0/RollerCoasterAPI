using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Controllers.Game;

[Route("[controller]")]
[ApiController]
public class ItemController(IItemService itemService): ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody, FromQuery] ItemCreationDTO itemCreationDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var createdItemId = await itemService.Create(int.Parse(userId), itemCreationDto);
        return Created("", new
        {
            Id = createdItemId
        });
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await itemService.Delete(int.Parse(userId), id);
        return Ok();
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var itemDto = await itemService.Get(id);
        return Ok(itemDto);
    }
}