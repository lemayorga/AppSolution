using SG.Domain.Base;

namespace SG.Domain.Entities.Commun;

public class Catalogue : BaseEntity<int>
{
    public string Value { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int? IdCatalogueHigher { get; set; }
    public int Orden { get; set; }
    public  Catalogue? CatalogueHigher { get; set; }
    public  ICollection<Catalogue> CatalogueChildren { get; } = [];
}
