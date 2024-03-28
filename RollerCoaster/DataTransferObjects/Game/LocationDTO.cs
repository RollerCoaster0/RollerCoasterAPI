namespace RollerCoaster.DataTransferObjects.Game;

public class LocationDTO
{
    public required string Name { get; set; }
    // уникальное поле для идентификации в запросе, ничего общего с БД не имеет
    public required string Id { get; set; }
    public required string Description { get; set; }
}