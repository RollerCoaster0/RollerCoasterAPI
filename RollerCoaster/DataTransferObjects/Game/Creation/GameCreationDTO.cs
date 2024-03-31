namespace RollerCoaster.DataTransferObjects.Game;

/* TODO: проработать загрузку файлов для локаций
   Сделаем это следующим образом:
   1. В запросе сущностям локаций клиент дает идентификатор;
   2. После выполнения запроса сервер возвращает URL, на который клиент в формате multipart-form-data
      загружает карты. Ключ - идентификатор локации (из этапа 1), значение - байты картинки.
*/ 

public class GameCreationDTO
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required List<LocationCreationDTO> Locations { get; set; }
    public required List<ItemDTO> Items { get; set; }
    public required List<QuestDTO> Quests { get; set; }
    public required List<NonPlayableCharacterCreationDTO> NonPlayableCharacters { get; set; }
    public required List<CharacterClassCreationDTO> Classes { get; set; }
    public required List<SkillCreationDTO> Skills { get; set; }
}