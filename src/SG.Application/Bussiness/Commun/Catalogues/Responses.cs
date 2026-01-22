using SG.Domain.Entities.Commun;
using SG.Shared.Responses;

namespace SG.Application.Bussiness.Commun.Catalogues.Responses;

public  class CatalogueResponse : BaseWithIdResponse
{
    public string Value { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int? IdCatalogueHigher { get; set; }
    public int Orden { get; set; }
}

public class CatalogueWithChildrenResponse   
{
    public int Id { get; set;   }
    public int Level { get; set; }
    public string Value { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int? IdCatalogueHigher { get; set; }
    public int Orden { get; set; }

    public IEnumerable<CatalogueWithChildrenResponse> Children { get; set; } = [];
}