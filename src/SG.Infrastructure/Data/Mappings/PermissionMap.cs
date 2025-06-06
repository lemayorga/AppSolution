using System;
using Microsoft.EntityFrameworkCore;
using SG.Domain.Security.Entities;

namespace SG.Infrastructure.Data.Mappings;

public class PermissionMap : IEntityTypeConfiguration<Permission>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");
        builder.Property(c => c.Id);
        builder.Property(c => c.IdRol).IsRequired();
        builder.Property(c => c.IdAction).IsRequired();
        builder.Property(c => c.IdModule).IsRequired();
        builder.Property(c => c.IsActive).IsRequired().HasDefaultValue(true);

        builder.HasOne(u => u.Action)
            .WithMany(u => u.Permissions)
            .HasForeignKey(u => u.IdAction)
            .OnDelete(DeleteBehavior.ClientNoAction)
            .HasConstraintName("FK_Permission_Actions");


        builder.HasOne(u => u.Module)
            .WithMany(u => u.Permissions)
            .HasForeignKey(u => u.IdModule)
            .OnDelete(DeleteBehavior.ClientNoAction)
            .HasConstraintName("FK_Permission_Modules");
    }
}