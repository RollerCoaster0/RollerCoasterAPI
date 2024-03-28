namespace RollerCoaster.DataTransferObjects.Game;

/* TODO: проработать загрузку файлов для локаций
   Сделаем это следующим образом:
   1. В запросе сущностям локаций клиент дает идентификатор;
   2. После выполнения запроса сервер возвращает URL, на который клиент в формате multipart-form-data
      загружает карты. Ключ - идентификатор локации (из этапа 1), значение - байты картинки.
*/ 

public class GameLocationDTO
{
    public required string Name { get; set; }
    // уникальное поле для идентификации в запросе, ничего общего с БД не имеет
    public required string Id { get; set; }
    public required string Description { get; set; }
}

public class GameItemDTO
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string ItemType { get; set; } // TODO: сделать так, чтобы автоматически конвертило в енам
}

public class GameQuestDTO
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string HiddenDescription { get; set; }
}

public class GameNonPlayableCharacterDTO 
{
    // здесь указывается идентификатор локации, в которой будет изначально "заспавнен" нпс
    public required string BaseLocationId { get; set; }
    public required string Name { get; set; }
}

public class GameCharacterClassDTO 
{
    public required string Name { get; set; }
    // уникальное поле для идентификации в запросе, ничего общего с БД не имеет
    public required string Id { get; set; }
    public required string Description { get; set; }
}

public class GameSkillDTO
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    // здесь указывается идентификатор класса, если скилл уникален для этого класса
    public required string? ForClassId { get; set; }
}

public class CreateGameDTO
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required List<GameLocationDTO> Locations { get; set; }
    public required List<GameItemDTO> Items { get; set; }
    public required List<GameQuestDTO> Quests { get; set; }
    public required List<GameNonPlayableCharacterDTO> NonPlayableCharacters { get; set; }
    public required List<GameCharacterClassDTO> Classes { get; set; }
    public required List<GameSkillDTO> Skills { get; set; }
}