using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Controllers.Game;

[Route("[controller]")]
[ApiController]
public class GameController(IGameService gameService): ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody, FromQuery] GameCreationDTO gameCreationDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var createdGameId = await gameService.Create(int.Parse(userId), gameCreationDto);
        return Created("", new
        {
            Id = createdGameId
        });
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        await gameService.Delete(int.Parse(userId), id);
        return Ok();
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var gameDto = await gameService.Get(id);
        return Ok(gameDto);
    }
}