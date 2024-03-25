using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RollerCoaster.Services;

namespace RollerCoaster.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class UsersController(IUsersService usersService): ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var user = await usersService.GetMe(int.Parse(userId));

        if (user is null)
            return NotFound(new
            {
                Message = "Пользователь не зарегистрирован"
            });
        
        return Ok(user);
    }
}