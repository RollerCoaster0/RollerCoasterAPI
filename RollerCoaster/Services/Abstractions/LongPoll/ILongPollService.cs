using RollerCoaster.DataTransferObjects.Updates;

namespace RollerCoaster.LongPoll;

public interface ILongPollService
{
    Task<LongPollUpdate> DequeueUpdateAsync(int userId);
    
    Task EnqueueUpdateAsync(int userId, LongPollUpdate update);
}