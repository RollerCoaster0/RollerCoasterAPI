namespace RollerCoaster.DataTransferObjects.Session.Fetching;

public class SessionDTO
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int CreatorId { get; set; }
    public required DateTimeOffset CreationDate { get; set; }
}