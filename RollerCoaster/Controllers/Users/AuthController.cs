using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Users;
using RollerCoaster.Services.Abstractions.Users;

namespace RollerCoaster.Controllers.Users;

[Route("auth")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType<AuthorizedUserMeta>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthorizedUserMeta>> Register([FromQuery] RegisterDTO registerDto)
    {
        var authorizedUser = await authService.Register(registerDto);
        return Created("", authorizedUser);
    }
    
    [HttpPost("login")]
    [ProducesResponseType<AuthorizedUserMeta>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<AuthorizedUserMeta>> Login([FromQuery] LoginDTO loginDto)
    {
        var authorizedUser = await authService.Login(loginDto);
        return Ok(authorizedUser);
    }
}