namespace RollerCoaster.DataTransferObjects.Game.Creation;

public class ItemCreationDTO
{
    public required int GameId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string ItemType { get; set; } // TODO: сделать так, чтобы автоматически конвертило в енам
}
