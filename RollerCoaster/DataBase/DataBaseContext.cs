using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase.Models;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataBase.Models.Session;
using NonPlayableCharacter = RollerCoaster.DataBase.Models.Game.NonPlayableCharacter;

namespace RollerCoaster.DataBase;

public sealed class DataBaseContext: DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;
    public DbSet<Game> Games { get; set; } = null!;
    public DbSet<Location> Locations { get; set; } = null!;
    public DbSet<Item> Items { get; set; } = null!;
    public DbSet<Skill> Skills { get; set; } = null!;
    public DbSet<NonPlayableCharacter> NonPlayableCharacters { get; set; } = null!;
    public DbSet<CharacterClass> CharacterClasses { get; set; } = null!;
    public DbSet<Quest> Quests { get; set; } = null!;
    public DbSet<Attributes> Attributes { get; set; } = null!;
    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<ActiveNonPlayableCharacter> ActiveNonPlayableCharacters { get; set; } = null!;
    public DbSet<InventoryItem> Inventories { get; set; } = null!;
    public DbSet<Session> Sessions { get; set; } = null!;
    public DbSet<QuestStatus> QuestStatuses { get; set; } = null!;
    public DbSet<UsedSkillAction> UsedSkillActions { get; set; } = null!;
    public DbSet<UsedRollAction> UsedRollActions { get; set; } = null!;

    public DataBaseContext(DbContextOptions<DataBaseContext> options): base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // thanks god https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-many
        
        modelBuilder.Entity<NonPlayableCharacter>()
            .HasMany<ActiveNonPlayableCharacter>()
            .WithOne()
            .HasForeignKey(e => e.NonPlayableCharacterId)
            .IsRequired();
        
        modelBuilder.Entity<Quest>()
            .HasMany<QuestStatus>()
            .WithOne()
            .HasForeignKey(e => e.QuestId)
            .IsRequired();
        
        modelBuilder.Entity<Session>()
            .HasMany<Message>()
            .WithOne()
            .HasForeignKey(e => e.SessionId)
            .IsRequired();
        
        modelBuilder.Entity<Player>()
            .HasMany<Message>()
            .WithOne()
            .HasForeignKey(e => e.SenderPlayerId)
            .IsRequired();
    }
}