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
        builder.Property(c => c.Id);
        builder.Property(c => c.Value).HasMaxLength(DataSchemaConstants.DEFAULT_LENGTH_MEDIUM_TEXT).IsRequired();
        builder.Property(c => c.Code).HasMaxLength(DataSchemaConstants.DEFAULT_LENGTH_EXTRA_SMALL_TEXT).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(DataSchemaConstants.DEFAULT_MAX_LENGTH_TEXT).IsRequired(false);
        builder.Property(c => c.IsActive).HasDefaultValue(true).IsRequired();
        builder.Property(c => c.IdCatalogueHigher).HasColumnName("IdCatalogueHigher").IsRequired(false);
        builder.Property(c => c.Orden).HasDefaultValue(0);

        builder
            .HasOne(o => o.CatalogueHigher)
            .WithMany(c => c.CatalogueChildren)
            .HasForeignKey(o => o.IdCatalogueHigher)
            .HasConstraintName("FK_Catalogue_Catalogue_IdCatalogueHigher");
    }
}