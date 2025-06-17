using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SG.Domain.Security.Entities;
using SG.Shared.Constants;

namespace SG.Infrastructure.Data.Mappings;

public class UsersTokenMap : IEntityTypeConfiguration<UsersToken>
{
    public void Configure(EntityTypeBuilder<UsersToken> builder)
    {
        builder.ToTable("UsersToken");
        builder.Property(c => c.Id).HasColumnOrder(0);
        builder.Property(c => c.IdUser).HasColumnOrder(1).IsRequired();
        builder.Property(c => c.RefreshToken).HasColumnOrder(2).HasMaxLength(DataSchemaConstants.DEFAULT_MAX_LENGTH_TEXT).IsRequired();
        builder.Property(c => c.RefreshTokenExpiry).HasColumnOrder(3).IsRequired();     

        builder.HasOne(u => u.User)
            .WithOne(u => u.UserToken)
            .HasPrincipalKey<User>(p => p.Id)
            .HasForeignKey<UsersToken>(p => p.IdUser)
            .HasConstraintName("FK_UsersToken_User");  
    }
}
 