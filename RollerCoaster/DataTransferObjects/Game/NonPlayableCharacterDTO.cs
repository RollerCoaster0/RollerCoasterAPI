namespace RollerCoaster.DataTransferObjects.Game;

public class NonPlayableCharacterDTO 
{
    // здесь указывается идентификатор локации, в которой будет изначально "заспавнен" нпс
    public required string BaseLocationId { get; set; }
    public required string Name { get; set; }
}