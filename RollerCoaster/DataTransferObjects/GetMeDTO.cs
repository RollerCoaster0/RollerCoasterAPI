namespace RollerCoaster.DataTransferObjects;

public class GetMeDTO
{
    public required int UserId { get; set; }
    public required string Login { get; set; }
}