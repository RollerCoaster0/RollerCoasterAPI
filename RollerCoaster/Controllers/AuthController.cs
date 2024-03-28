using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects;
using RollerCoaster.DataTransferObjects.Users;
using RollerCoaster.Services;

namespace RollerCoaster.Controllers;

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
        catch (InvalidLoginError e)
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
        catch (InvalidLoginError e)
        {
            return BadRequest(new { e.Message });
        }
    }
}