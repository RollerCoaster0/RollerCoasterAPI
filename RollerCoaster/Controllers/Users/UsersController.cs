using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Users;
using RollerCoaster.Services.Abstractions.Users;

namespace RollerCoaster.Controllers.Users;

[Route("users")]
[ApiController]
[Authorize]
public class UsersController(IUsersService usersService): ControllerBase
{
    [HttpGet("me")]
    [ProducesResponseType<GetMeDTO>(StatusCodes.Status200OK)]
    public async Task<ActionResult<GetMeDTO>> Me()
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var user = await usersService.GetMe(int.Parse(userId));
    
        return Ok(user);
    }
}