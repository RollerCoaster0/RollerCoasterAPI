using Microsoft.EntityFrameworkCore;
using RollerCoaster.Models;

namespace RollerCoaster;

public sealed class DataBaseContext(DbContextOptions<DataBaseContext> options) : DbContext(options)
{
    public required DbSet<User> Users { get; set; }
}