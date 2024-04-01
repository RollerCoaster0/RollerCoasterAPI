namespace RollerCoaster.DataTransferObjects.Game.Creation;

public class LocationCreationDTO
{
    public required int GameId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}