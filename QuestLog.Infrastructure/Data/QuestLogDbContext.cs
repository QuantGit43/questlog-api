using Microsoft.EntityFrameworkCore;
using QuestLog.Domain.Entities;
using Task = QuestLog.Domain.Entities.Task;

namespace QuestLog.Infrastructure.Data;

public class QuestLogDbContext: DbContext
{
    public QuestLogDbContext(DbContextOptions<QuestLogDbContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<Avatar> Avatars { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Avatar>()
            .Property(p => p.Class)
            .HasConversion<string>();
        
        modelBuilder.Entity<Task>()
            .Property(p => p.Type)
            .HasConversion<string>();
        
        modelBuilder.Entity<User>()
            .HasOne(u => u.Avatar)
            .WithOne(a => a.User)
            .HasForeignKey<Avatar>(a => a.UserId);

        modelBuilder.Entity<Avatar>()
            .HasMany(a => a.Tasks)
            .WithOne(t => t.Owner)
            .HasForeignKey(q => q.OwnerAvatarId);
    }
   
    
}