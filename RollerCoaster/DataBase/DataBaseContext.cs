using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase.Models;

namespace RollerCoaster.DataBase;

public sealed class DataBaseContext: DbContext 
{
    public required DbSet<User> Users { get; set; }

    public DataBaseContext(DbContextOptions<DataBaseContext> options): base(options)
    {
        Database.EnsureCreated();
    }
}