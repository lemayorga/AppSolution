using Microsoft.EntityFrameworkCore;
using SG.Domain.Security.Entities;

namespace SG.Infrastructure.Data.Mappings;

public class UsersRolesMap : IEntityTypeConfiguration<UsersRoles>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<UsersRoles> builder)
    {
        builder.ToTable("UsersRoles");
        builder.Property(c => c.Id);
        builder.Property(c => c.IdRol).IsRequired();
        builder.Property(c => c.IdUser).IsRequired();
        builder.Property(c => c.State).IsRequired().HasDefaultValue(true);

        builder.HasOne(u => u.User)
            .WithMany(u => u.UsersRoles)
            .HasForeignKey(u => u.IdUser)
            .OnDelete(DeleteBehavior.ClientNoAction)
            .HasConstraintName("FK_UsersRoles_User");

        builder.HasOne(u => u.Role)
            .WithMany(u => u.UsersRoles)
            .HasForeignKey(u => u.IdRol)
            .OnDelete(DeleteBehavior.ClientNoAction)
            .HasConstraintName("FK_UsersRoles_Role");
    }
}