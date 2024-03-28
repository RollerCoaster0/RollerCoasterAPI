namespace RollerCoaster.DataTransferObjects.Users;

public class GetMeDTO
{
    public required int UserId { get; set; }
    public required string Login { get; set; }
}