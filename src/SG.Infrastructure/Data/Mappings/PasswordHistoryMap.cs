using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SG.Domain.Security.Entities;
using SG.Infrastructure.Data.Config;
namespace SG.Infrastructure.Data.Mappings;

public class PasswordHistoryMap : IEntityTypeConfiguration<PasswordHistory>
{
    public void Configure(EntityTypeBuilder<PasswordHistory> builder)
    {
        builder.ToTable("PasswordHistory");
        builder.Property(c => c.Id).HasColumnOrder(0);
        builder.Property(c => c.Username).HasColumnOrder(1).HasMaxLength(DataSchemaConstants.DEFAULT_LENGTH_SHORT_TEXT).IsRequired();
        builder.Property(c => c.OldPassword).HasColumnOrder(2).HasMaxLength(DataSchemaConstants.DEFAULT_MAX_LENGTH_TEXT_CSharp).IsRequired();
        builder.Property(c => c.DateChange).HasColumnOrder(3).IsRequired();
        builder.Property(c => c.UserSave).HasColumnOrder(4).HasMaxLength(DataSchemaConstants.DEFAULT_LENGTH_SHORT_TEXT).IsRequired();
    }
}
 