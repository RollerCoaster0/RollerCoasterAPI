﻿namespace RollerCoaster.DataTransferObjects.Game.Fetching;

public class CharacterClassDTO 
{
    public required int Id { get; set; }
    public required int GameId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}