using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Controllers.Game;

[Route("[controller]")]
[ApiController]
public class GameController(IGameService gameService): ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(GameCreationDTO gameCreationDto)
    {
        try
        {
            var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
            var createdGameId = await gameService.Create(int.Parse(userId), gameCreationDto);
            return Created("", new
            {
                Id = createdGameId
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
            await gameService.Delete(int.Parse(userId), id);
            return Ok();
        }
        catch (NotFoundError e)
        {
            return NotFound(new { e.Message });
        }
        catch (AccessDeniedError e)
        {
            return Forbid(e.Message);
        }
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var gameDto = await gameService.Get(id);
            return Ok(gameDto);
        }
        catch (NotFoundError e)
        {
            return NotFound(new { e.Message });
        }
    }
}