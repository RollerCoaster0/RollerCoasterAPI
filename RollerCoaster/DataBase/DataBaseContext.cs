using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase.Models;
using RollerCoaster.DataBase.Models.Chat;

namespace RollerCoaster.DataBase;

public sealed class DataBaseContext: DbContext 
{
    public required DbSet<User> Users { get; set; }
    public required DbSet<Message> Messages { get; set; }

    public DataBaseContext(DbContextOptions<DataBaseContext> options): base(options)
    {
        Database.EnsureCreated();
    }
}