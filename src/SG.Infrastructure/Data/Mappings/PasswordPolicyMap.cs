using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SG.Domain.Entities.Security;
using SG.Shared.Constants;
namespace SG.Infrastructure.Data.Mappings;

public class PasswordPolicyMap : IEntityTypeConfiguration<PasswordPolicy>
{
    public void Configure(EntityTypeBuilder<PasswordPolicy> builder)
    {
        builder.ToTable("PasswordPolicy");
        builder.Property(c => c.Id).HasColumnOrder(0).IsRequired();
        builder.Property(c => c.Code).HasColumnOrder(1).HasMaxLength(DataSchemaConstants.DEFAULT_LENGTH_SMALL_TEXT).IsRequired();
        builder.Property(c => c.Value).HasColumnOrder(2).HasMaxLength(DataSchemaConstants.DEFAULT_LENGTH_TINY_TEXT).IsRequired();
        builder.Property(c => c.Description).HasColumnOrder(3).HasMaxLength(DataSchemaConstants.DEFAULT_LENGTH_LARGE_TEXT).IsRequired(false);
    }
}
