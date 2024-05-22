namespace RollerCoaster.DataTransferObjects.Game.Fetching;

public class GameDTO
{
    public required int Id { get; set; }
    public required int CreatorId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int? BaseLocationId { get; set; }
    public required List<LocationDTO> Locations { get; set; }
    public required List<CharacterClassDTO> Classes { get; set; }
    public required List<QuestDTO> Quests { get; set; }
    public required List<ItemDTO> Items { get; set; }
    public required List<NonPlayableCharacterDTO> NonPlayableCharacters { get; set; }
    public required List<SkillDTO> Skills { get; set; }
}