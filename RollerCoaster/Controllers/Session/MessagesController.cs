using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Common;
using RollerCoaster.DataTransferObjects.Session.Messages;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Controllers.Session;

[Route("messages")]
[ApiController]
[Authorize]
public class MessagesController(IMessageService messageService): ControllerBase
{
    [HttpPost("sendText")]
    [ProducesResponseType<IdOfCreatedObjectDTO>(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IdOfCreatedObjectDTO>> SendTextMessage([FromQuery] SendTextMessageDTO sendTextMessageDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var createdMessageId = await messageService.SendTextMessage(int.Parse(userId), sendTextMessageDto);
        return Created("", new IdOfCreatedObjectDTO { Id = createdMessageId });
    }
    
    [HttpGet]
    [ProducesResponseType<List<MessageDTO>>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<MessageDTO>>> GetMessages([FromQuery] GetMessagesDTO getMessagesDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var messages = await messageService.GetBySession(int.Parse(userId), getMessagesDto);
        return Ok(messages);
    }
}