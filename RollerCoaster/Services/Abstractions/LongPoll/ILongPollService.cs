using RollerCoaster.DataTransferObjects.LongPoll;

namespace RollerCoaster.Services.Abstractions.LongPoll;

public interface ILongPollService
{
    Task<LongPollUpdate> DequeueUpdateAsync(int userId);
    
    Task EnqueueUpdateAsync(int userId, LongPollUpdate update);
}