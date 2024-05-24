using RollerCoaster.DataTransferObjects.Session.Chat;

namespace RollerCoaster.Services.Abstractions.Sessions;

public interface IChatService
{
    public Task<List<ChatActionDTO>> GetActions(int accessorUserId, GetChatActionsDTO getChatActionsDto);
}