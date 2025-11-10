using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuestLog.Domain.Entities;

namespace QuestLog.Infrastructure.Data.Configuration;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Username)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.HashedPassword)
            .IsRequired();

        builder.HasIndex(x => x.Username)
            .IsUnique();
        
        builder.HasIndex(x => x.Email)
            .IsUnique();
        
        builder.HasOne(x => x.Avatar)
            .WithOne(x => x.User)
            .HasForeignKey<Avatar>(x => x.UserId);
    }
}