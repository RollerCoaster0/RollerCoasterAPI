using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Sessions;

namespace RollerCoaster.Services.Implementations.Session;

public class RollService: IRollService
{
    private static readonly int[] AvailableDice = [4, 6, 8, 10, 12, 20, 100];
    
    public Task<int> Roll(int die)
    {
        if (!AvailableDice.Contains(die))
            throw new NotFoundError("Дайс с заданным количеством граней не найден.");

        return Task.FromResult(Random.Shared.Next(1, die + 1));
    }
}