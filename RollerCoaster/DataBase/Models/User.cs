using System.ComponentModel.DataAnnotations.Schema;

namespace RollerCoaster.DataBase.Models;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required string Login { get; set; }
    public required string PasswordHash { get; set; }
}