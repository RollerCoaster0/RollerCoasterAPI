using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Common;
using RollerCoaster.DataTransferObjects.Session.Chat.Messages;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Controllers.Session;

[Route("messages")]
[ApiController]
[Authorize]
public class MessagesController(IMessageService messageService): ControllerBase
{
    [HttpPost]
    [ProducesResponseType<IdOfCreatedObjectDTO>(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IdOfCreatedObjectDTO>> Create([FromQuery] SendMessageDTO sendMessageDto)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var createdMessageId = await messageService.Create(int.Parse(userId), sendMessageDto);
        return Created("", new IdOfCreatedObjectDTO { Id = createdMessageId });
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType<MessageDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageDTO>> Get(int id)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var message = await messageService.Get(int.Parse(userId), id);
        return Ok(message);
    }
}