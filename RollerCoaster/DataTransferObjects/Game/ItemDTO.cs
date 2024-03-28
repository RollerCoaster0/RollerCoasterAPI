namespace RollerCoaster.DataTransferObjects.Game;

public class ItemDTO
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string ItemType { get; set; } // TODO: сделать так, чтобы автоматически конвертило в енам
}
