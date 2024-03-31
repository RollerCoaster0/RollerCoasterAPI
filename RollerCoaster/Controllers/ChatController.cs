using Microsoft.AspNetCore.Mvc;
using RollerCoaster.DataTransferObjects.Chat;
using RollerCoaster.Services;

namespace RollerCoaster.Controllers;

[Route("[controller]")]
[ApiController]
public class ChatController(IChatService chatService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Send(SendMessageDTO sendMessageDto)
    {
        var senderId = HttpContext.User.Claims.First(c => c.Type == "id").Value;
        var messageId = await chatService.SendMessage(int.Parse(senderId), sendMessageDto);
        return Created("", new
        {
            Id = messageId
        });
    }

    [HttpGet("last")]
    public async Task<IActionResult> GetLast(GetLastMessagesDTO getLastMessagesDto)
    {
        var messages = await chatService.GetLastMessages(getLastMessagesDto);
        return Ok(messages);
    }
}