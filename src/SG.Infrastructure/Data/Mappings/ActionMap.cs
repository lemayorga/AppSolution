using Microsoft.EntityFrameworkCore;
using SG.Shared.Constants;
using Action = SG.Domain.Entities.Security.Action;

namespace SG.Infrastructure.Data.Mappings;

public class ActionMap : IEntityTypeConfiguration<Action>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Action> builder)
    {
        builder.ToTable("Action");
        builder.Property(c => c.Id);
        builder.Property(c => c.Name).HasMaxLength(DataSchemaConstants.DEFAULT_LENGTH_EXTRA_SMALL_TEXT).IsRequired();
        builder.Property(c => c.IsActive).IsRequired().HasDefaultValue(true);
    }
}
