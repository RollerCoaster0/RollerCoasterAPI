namespace RollerCoaster.DataTransferObjects.LongPoll;

public class LongPollResponse
{
    public required string DeviceId { get; set; }
    public required LongPollUpdate Update { get; set; }
}