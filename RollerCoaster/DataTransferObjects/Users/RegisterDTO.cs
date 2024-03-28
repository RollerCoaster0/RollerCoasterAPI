namespace RollerCoaster.DataTransferObjects.Users;

public class RegisterDTO
{
    public required string Login { get; set; }
    public required string Password { get; set; }
}