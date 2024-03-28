using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase.Models;
using RollerCoaster.DataBase.Models.Game;

namespace RollerCoaster.DataBase;

public sealed class DataBaseContext(DbContextOptions<DataBaseContext> options) : DbContext(options)
{
    public required DbSet<User> Users { get; set; }
    public required DbSet<Game> Games { get; set; }
}