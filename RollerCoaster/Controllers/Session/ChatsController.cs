using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Chat;
using RollerCoaster.DataTransferObjects.Common;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Controllers.Session;

[Route("chats")]
[ApiController]
[Authorize]
public class ChatsController(IChatService chatService) : ControllerBase
{
    // TODO: type hints
    
    [HttpPost]
    public async Task<ActionResult<IdOfCreatedObjectDTO>> Send([FromQuery] SendMessageDTO sendMessageDto)
    {
        var senderId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var messageId = await chatService.SendMessage(int.Parse(senderId), sendMessageDto);
        return Created("", new IdOfCreatedObjectDTO { Id = messageId });
    }

    [HttpGet("last")]
    public async Task<ActionResult<List<MessageDTO>>> GetLast([FromQuery] GetLastMessagesDTO getLastMessagesDto)
    {
        var messages = await chatService.GetLastMessages(getLastMessagesDto);
        return Ok(messages);
    }
}