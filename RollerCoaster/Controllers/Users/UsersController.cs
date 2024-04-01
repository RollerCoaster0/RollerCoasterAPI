using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Users;

namespace RollerCoaster.Controllers.Users;

[Route("[controller]")]
[ApiController]
[Authorize]
public class UsersController(IUsersService usersService): ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        try
        {
            var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
            var user = await usersService.GetMe(int.Parse(userId));
        
            return Ok(user);
        }
        catch (NotFoundError e)
        {
            return NotFound(new { e.Message });
        }
    }
}