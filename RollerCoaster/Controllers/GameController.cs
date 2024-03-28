using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Game;
using RollerCoaster.Services;

namespace RollerCoaster.Controllers;

[Route("[controller]")]
[ApiController]
public class GameController(IGameService gameService): ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateGameDTO gameDto)
    {
        var createdGameId = await gameService.Create(gameDto);
        return Created("", new
        {
            Id = createdGameId
        });
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await gameService.Delete(id);
        return Ok();
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var gameDto = await gameService.Get(id);

        if (gameDto is null)
            return NotFound();
        
        return Ok(gameDto);
    }
}