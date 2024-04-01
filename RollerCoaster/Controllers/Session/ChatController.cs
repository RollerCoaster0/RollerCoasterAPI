using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Chat;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Controllers.Session;

[Route("[controller]")]
[ApiController]
public class ChatController(IChatService chatService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Send([FromQuery] SendMessageDTO sendMessageDto)
    {
        try
        {
            var senderId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
            var messageId = await chatService.SendMessage(int.Parse(senderId), sendMessageDto);
            return Created("", new
            {
                Id = messageId
            });
        }
        catch (ProvidedDataIsInvalidError e)
        {
            return BadRequest(new { e.Message });
        }
    }

    [HttpGet("last")]
    public async Task<IActionResult> GetLast([FromQuery] GetLastMessagesDTO getLastMessagesDto)
    {
        try
        {
            var messages = await chatService.GetLastMessages(getLastMessagesDto);
            return Ok(messages);
        }
        catch (ProvidedDataIsInvalidError e)
        {
            return BadRequest(new { e.Message });
        }
    }
}