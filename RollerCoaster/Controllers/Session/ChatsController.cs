using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Chat;
using RollerCoaster.DataTransferObjects.Common;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Controllers.Session;

[Route("chats")]
[ApiController]
public class ChatsController(IChatService chatService) : ControllerBase
{
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