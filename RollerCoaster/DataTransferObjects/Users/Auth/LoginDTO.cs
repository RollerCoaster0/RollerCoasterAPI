namespace RollerCoaster.DataTransferObjects.Users.Auth;

// TODO: move validation here

public class LoginDTO
{
    public required string Login { get; set; }
    public required string Password { get; set; }
}