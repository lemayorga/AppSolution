using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SG.Domain.Entities.Commun;
using SG.Shared.Constants;

namespace SG.Infrastructure.Data.Mappings;

public class CatalogueMap : IEntityTypeConfiguration<Catalogue>
{
    public void Configure(EntityTypeBuilder<Catalogue> builder)
    {
        builder.ToTable("Catalogue");
        builder.Property(c => c.Id).HasColumnOrder(0);
        builder.Property(c => c.Group).HasColumnOrder(1).HasMaxLength(DataSchemaConstants.DEFAULT_MAX_LENGTH_TEXT).IsRequired();
        builder.Property(c => c.Value).HasColumnOrder(2).HasMaxLength(200).IsRequired();
        builder.Property(c => c.IsActive).HasColumnOrder(3).HasDefaultValue(true).IsRequired();
        builder.Property(c => c.Description).HasColumnOrder(4).HasMaxLength(DataSchemaConstants.DEFAULT_MAX_LENGTH_TEXT).IsRequired(false);
    }
}