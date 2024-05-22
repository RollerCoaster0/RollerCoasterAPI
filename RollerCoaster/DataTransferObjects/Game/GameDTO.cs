using RollerCoaster.DataTransferObjects.Game.CharacterClasses;
using RollerCoaster.DataTransferObjects.Game.Items;
using RollerCoaster.DataTransferObjects.Game.Locations;
using RollerCoaster.DataTransferObjects.Game.NonPlayableCharacters;
using RollerCoaster.DataTransferObjects.Game.Quests;
using RollerCoaster.DataTransferObjects.Game.Skills;

namespace RollerCoaster.DataTransferObjects.Game;

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