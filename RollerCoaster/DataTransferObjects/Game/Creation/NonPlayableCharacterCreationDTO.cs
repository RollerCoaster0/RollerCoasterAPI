namespace RollerCoaster.DataTransferObjects.Game;

public class NonPlayableCharacterCreationDTO 
{
    public required string Name { get; set; }
    // здесь указывается идентификатор локации, в которой будет изначально "заспавнен" нпс
    public required string BaseLocationTemporaryResolutionId { get; set; }
}