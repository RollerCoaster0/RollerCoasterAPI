using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase.Models;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataBase.Models.Session.Chat;

namespace RollerCoaster.DataBase;

public sealed class DataBaseContext: DbContext
{
    public required DbSet<User> Users { get; set; }
    public required DbSet<Message> Messages { get; set; }
    public required DbSet<Game> Games { get; set; }
    public required DbSet<Location> Locations { get; set; }
    public required DbSet<Item> Items { get; set; }
    public required DbSet<Skill> Skills { get; set; }
    public required DbSet<NonPlayableCharacter> NonPlayableCharacters { get; set; }
    public required DbSet<CharacterClass> CharacterClasses { get; set; }
    public required DbSet<Quest> Quests { get; set; }

    public DataBaseContext(DbContextOptions<DataBaseContext> options): base(options)
    {
        Database.EnsureCreated();
    }
}