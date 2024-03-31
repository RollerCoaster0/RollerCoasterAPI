namespace RollerCoaster.DataTransferObjects.Game;

public class LocationCreationDTO
{
    // уникальное поле для идентификации в запросе, ничего общего с БД не имеет
    public required string TemporaryResolutionId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}