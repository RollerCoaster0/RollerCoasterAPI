using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase.Models;

namespace RollerCoaster.DataBase;

public sealed class DataBaseContext(DbContextOptions<DataBaseContext> options) : DbContext(options)
{
    public required DbSet<User> Users { get; set; }
}