using Microsoft.AspNetCore.Mvc;

namespace RollerCoaster.Controllers.Game;

[Route("[controller]")]
[ApiController]
public class ItemController(): ControllerBase
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