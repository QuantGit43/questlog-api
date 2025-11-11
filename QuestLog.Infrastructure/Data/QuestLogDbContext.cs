using System.Reflection;
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

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
   
    
}