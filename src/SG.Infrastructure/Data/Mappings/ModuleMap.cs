using Microsoft.EntityFrameworkCore;
using SG.Shared.Constants;
using Module = SG.Domain.Security.Entities.Module;

namespace SG.Infrastructure.Data.Mappings;

    public class ModuleMap : IEntityTypeConfiguration<Module>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Module> builder)
        {
            builder.ToTable("Module");
            builder.Property(c => c.Id);
            builder.Property(c => c.Name).HasMaxLength(DataSchemaConstants.DEFAULT_LENGTH_NAMES).IsRequired();
            builder.Property(c => c.Description).HasMaxLength(DataSchemaConstants.DEFAULT_MAX_LENGTH_TEXT).IsRequired();
            builder.Property(c => c.IdParentModule).IsRequired(false);
            builder.Property(c => c.IsActive).IsRequired().HasDefaultValue(true);


            builder.HasMany(e => e.ChildrenModules)
                .WithOne(e => e.ParentModule)
                .HasForeignKey(e => e.IdParentModule)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Module_ParentModule");
        }
    }