using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Users;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Users;

namespace RollerCoaster.Controllers.Users;

[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpGet("register")]
    public async Task<IActionResult> Register([FromQuery] RegisterDTO registerDto)
    {
        try
        {
            var createdUser = await authService.Register(registerDto);
            return Created("", new
            {
                Token = createdUser.AccessToken,
                Id = createdUser.UserId
            });
        }
        catch (LoginAlreadyInUseError e)
        {
            return BadRequest(new { e.Message });
        }
        catch (ProvidedDataIsInvalidError e)
        {
            return BadRequest(new { e.Message });
        }
    }
    
    [HttpGet("login")]
    public async Task<IActionResult> Login([FromQuery] LoginDTO loginDto)
    {
        try
        {
            var token = await authService.Login(loginDto);
            return Ok(new
            {
                Token = token
            });
        }
        catch (InvalidLoginCredentialsError e)
        {
            return Forbid(e.Message);
        }
    }
}