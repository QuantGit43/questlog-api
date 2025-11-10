using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuestLog.Domain.Entities;

namespace QuestLog.Infrastructure.Data.Configuration
{
    public class AvatarConfiguration : IEntityTypeConfiguration<Avatar>
    {
        public void Configure(EntityTypeBuilder<Avatar> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.Class)
                .IsRequired()
                .HasConversion<string>();

            builder.ToTable(a => a.HasCheckConstraint("CK_Avatar_Level_Positive", "\"Level\" >= 1"));
            builder.ToTable(a => a.HasCheckConstraint("CK_Avatar_Gold_Positive", "\"Gold\" >= 0"));
            builder.ToTable(a => a.HasCheckConstraint("CK_Avatar_XP_Positive", "\"XP\" >= 0"));
            builder.ToTable(a => a.HasCheckConstraint("CK_Avatar_HP_Validation", "\"HP\" >= 0 AND \"HP\" <= \"MaxHP\""));
        }
    }
}