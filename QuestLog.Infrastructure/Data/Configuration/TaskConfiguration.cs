using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = QuestLog.Domain.Entities.Task;

namespace QuestLog.Infrastructure.Data.Configuration;

public class TaskConfiguration: IEntityTypeConfiguration<Task>
{
    public void Configure(EntityTypeBuilder<Task> builder)
    {
        builder.HasKey(q => q.Id);

            
        builder.Property(q => q.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(q => q.Description)
            .HasMaxLength(500); 

        builder.Property(q => q.Type)
            .IsRequired()
            .HasConversion<string>();
        
        builder.Property(q => q.XPReward)
            .IsRequired();
                
        builder.Property(q => q.GoldReward)
            .IsRequired();

        builder.ToTable(q => q.HasCheckConstraint("CK_Quest_XPReward_Positive", "\"XPReward\" >= 0"));
        builder.ToTable(q => q.HasCheckConstraint("CK_Quest_GoldReward_Positive", "\"GoldReward\" >= 0"));

       
        builder.HasOne(q => q.Avatar) 
            .WithMany(a => a.Tasks) 
            .HasForeignKey(q => q.OwnerAvatarId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}