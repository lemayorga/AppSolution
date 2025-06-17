using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SG.Domain.Entities.Security;
using SG.Shared.Constants;
namespace SG.Infrastructure.Data.Mappings;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder.Property(c => c.Id).HasColumnOrder(0);
        builder.Property(c => c.Username).HasColumnOrder(1).HasMaxLength(DataSchemaConstants.DEFAULT_LENGTH_SMALL_TEXT).IsRequired();
        builder.Property(c => c.Email).HasColumnOrder(2).HasMaxLength(DataSchemaConstants.DEFAULT_LENGTH_EMAILS).IsRequired();
        builder.Property(c => c.Firstname).HasColumnOrder(3).HasMaxLength(DataSchemaConstants.DEFAULT_LENGTH_SMALL_TEXT).IsRequired();
        builder.Property(c => c.Lastname).HasColumnOrder(4).HasMaxLength(DataSchemaConstants.DEFAULT_LENGTH_SMALL_TEXT).IsRequired();
        builder.Property(c => c.Password).HasColumnOrder(5).HasMaxLength(DataSchemaConstants.DEFAULT_MAX_LENGTH_TEXT_CSharp).IsRequired();
        builder.Property(c => c.IsActive).HasColumnOrder(6).HasDefaultValue(true).IsRequired();
        builder.Property(c => c.IsLocked).HasColumnOrder(7).HasDefaultValue(false).IsRequired();
    }
}
 