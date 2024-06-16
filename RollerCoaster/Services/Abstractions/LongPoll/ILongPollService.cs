using RollerCoaster.DataTransferObjects.LongPoll;

namespace RollerCoaster.Services.Abstractions.LongPoll;

public interface ILongPollService
{
    Task<LongPollResponse> DequeueUpdateAsync(int userId, string? deviceId);
    
    Task EnqueueUpdateAsync(int userId, LongPollUpdate update);
}