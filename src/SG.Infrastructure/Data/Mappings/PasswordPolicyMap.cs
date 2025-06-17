using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SG.Domain.Security.Entities;
using SG.Shared.Constants;
namespace SG.Infrastructure.Data.Mappings;

public class PasswordPolicyMap : IEntityTypeConfiguration<PasswordPolicy>
{
    public void Configure(EntityTypeBuilder<PasswordPolicy> builder)
    {
        builder.ToTable("PasswordPolicy");
        builder.Property(c => c.Id).IsRequired();
        builder.Property(c => c.MinimumDigits).IsRequired();
        builder.Property(c => c.RequiredLowercase).IsRequired();
        builder.Property(c => c.RequiredUppercase).IsRequired();
        builder.Property(c => c.RequiredCharacters).IsRequired();

        builder.Property(c => c.SpecialCharacters).HasMaxLength(DataSchemaConstants.DEFAULT_LENGTH_EXTRA_SHORT_TEXT).IsRequired();
        builder.Property(c => c.TimeNoRepeat).IsRequired();
        builder.Property(c => c.ChangeTimeType).HasMaxLength(1).IsRequired();
        builder.Property(c => c.PasswordChangeTime).IsRequired();
        builder.Property(c => c.TemporaryPasswordChangeTime).IsRequired();
    }
}
 