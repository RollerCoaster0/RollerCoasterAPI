namespace RollerCoaster.DataTransferObjects.Users;

// TODO: move validation here

public class RegisterDTO
{
    public required string Login { get; set; }
    public required string Password { get; set; }
}