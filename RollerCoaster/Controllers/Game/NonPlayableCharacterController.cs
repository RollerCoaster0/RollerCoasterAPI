using Microsoft.AspNetCore.Mvc;

namespace RollerCoaster.Controllers.Game;

[Route("npc")]
[ApiController]
public class NonPlayableCharacterController(): ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create()
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        throw new NotImplementedException();
    }
}