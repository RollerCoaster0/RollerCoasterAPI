using System.Collections.Concurrent;
using Microsoft.VisualStudio.Threading;
using RollerCoaster.DataTransferObjects.LongPoll;
using RollerCoaster.Services.Abstractions.LongPoll;

namespace RollerCoaster.Services.Implementations.LongPoll;

public class SimpleQueueLongPollService: ILongPollService
{
    private readonly ConcurrentDictionary<int, ConcurrentDictionary<string, AsyncQueue<LongPollUpdate>>> _queues = new();
    
    public async Task<LongPollResponse> DequeueUpdateAsync(int userId, string? deviceId)
    {
        var userQueues = _queues.GetOrAdd(userId, new ConcurrentDictionary<string, AsyncQueue<LongPollUpdate>>());

        deviceId ??= Guid.NewGuid().ToString("N");
        
        var queue = userQueues.GetOrAdd(deviceId, new AsyncQueue<LongPollUpdate>());
        return new LongPollResponse
        {
            Update = await queue.DequeueAsync(),
            DeviceId = deviceId 
        };
    }

    public Task EnqueueUpdateAsync(int userId, LongPollUpdate update)
    {
        var userQueues = _queues.GetOrAdd(userId, new ConcurrentDictionary<string, AsyncQueue<LongPollUpdate>>());

        foreach (var queue in userQueues.Values)
        {
            queue.Enqueue(update);    
        }
        
        return Task.CompletedTask;
    }
}