using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SG.Domain.Entities.Security;
using SG.Shared.Constants;
namespace SG.Infrastructure.Data.Mappings;

public class RoleMap : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Role");
        builder.Property(c => c.Id).HasColumnOrder(0);
        builder.Property(c => c.CodeRol).HasColumnOrder(1).HasMaxLength(DataSchemaConstants.DEFAULT_LENGTH_EXTRA_SMALL_TEXT).IsRequired();
        builder.Property(c => c.Name).HasColumnOrder(2).HasMaxLength(DataSchemaConstants.DEFAULT_LENGTH_SMALL_TEXT).IsRequired();
        builder.Property(c => c.IsActive).HasColumnOrder(3).HasDefaultValue(true).IsRequired();
    }
}
 