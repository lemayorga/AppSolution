using System;
using Microsoft.EntityFrameworkCore;
using SG.Infrastructure.Data.Config;
using Action = SG.Domain.Security.Entities.Action;

namespace SG.Infrastructure.Data.Mappings;

public class ActionMap : IEntityTypeConfiguration<Action>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Action> builder)
    {
        builder.ToTable("Action");
        builder.Property(c => c.Id);
        builder.Property(c => c.Name).HasMaxLength(DataSchemaConstants.DEFAULT_LENGTH_EXTRA_SMALL_TEXT).IsRequired();
        builder.Property(c => c.State).IsRequired().HasDefaultValue(true);
    }
}
