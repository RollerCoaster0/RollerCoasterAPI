using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Session.Chat;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Controllers.Session;

[Route("chats")]
[ApiController]
[Authorize]
public class ChatsController(IChatService chatService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<List<ChatActionDTO>>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<ChatActionDTO>>> GetActions([FromQuery] GetChatActionsDTO getChatActionsDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var messages = await chatService.GetActions(int.Parse(userId), getChatActionsDto);
        return Ok(messages);
    }
}