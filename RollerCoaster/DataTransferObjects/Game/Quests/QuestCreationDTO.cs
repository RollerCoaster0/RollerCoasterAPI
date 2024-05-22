using System.ComponentModel.DataAnnotations;

namespace RollerCoaster.DataTransferObjects.Game.Quests;

public class QuestCreationDTO
{
    public required int GameId { get; set; }
    [StringLength(64)] public required string Name { get; set; }
    [StringLength(512)] public required string Description { get; set; }
    [StringLength(512)] public required string HiddenDescription { get; set; }
}