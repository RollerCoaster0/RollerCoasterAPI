namespace RollerCoaster.Services.Abstractions.Sessions;

public interface IRollService
{
    public Task<int> Roll(int die);
}