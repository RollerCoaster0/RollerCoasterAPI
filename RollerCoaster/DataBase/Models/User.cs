using System.ComponentModel.DataAnnotations.Schema;
using RollerCoaster.DataBase.Models.Session;

namespace RollerCoaster.DataBase.Models;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required string Login { get; set; }
    public required string PasswordHash { get; set; }
    public List<Player> Players { get; set; } = [];
}