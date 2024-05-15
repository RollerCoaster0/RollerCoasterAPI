using System.Collections.Concurrent;
using Microsoft.VisualStudio.Threading;
using RollerCoaster.DataTransferObjects.Updates;

namespace RollerCoaster.LongPoll;

public class SimpleQueueLongPollService: ILongPollService
{
    private readonly ConcurrentDictionary<int, AsyncQueue<LongPollUpdate>> _queues = new();
    
    public async Task<LongPollUpdate> DequeueUpdateAsync(int userId)
    {
        var queue = _queues.GetOrAdd(userId, new AsyncQueue<LongPollUpdate>());
        return await queue.DequeueAsync();
    }

    public Task EnqueueUpdateAsync(int userId, LongPollUpdate update)
    {
        var queue = _queues.GetOrAdd(userId, new AsyncQueue<LongPollUpdate>());
        queue.Enqueue(update);
        
        return Task.CompletedTask;
    }
}