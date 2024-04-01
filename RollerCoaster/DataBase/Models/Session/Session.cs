namespace RollerCoaster.DataBase.Models.Session;

// сессия представляет собой запущенную игру
// TODO: доделать
public class Session
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int CreatorId { get; set; }
    public required DateTimeOffset CreationDate { get; set; }
}