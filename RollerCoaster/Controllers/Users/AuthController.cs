using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Users;
using RollerCoaster.Services.Abstractions.Users;

namespace RollerCoaster.Controllers.Users;

[Route("auth")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromQuery] RegisterDTO registerDto)
    {
        var createdUser = await authService.Register(registerDto);
        return Created("", new
        {
            Token = createdUser.AccessToken,
            Id = createdUser.UserId
        });
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromQuery] LoginDTO loginDto)
    {
        var token = await authService.Login(loginDto);
        return Ok(new
        {
            Token = token
        });
    }
}