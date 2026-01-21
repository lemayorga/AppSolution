namespace SG.Application.Bussiness.Commun.Catalogues.Requests;

public abstract class CatalogueBaseRequest
{
    public string Value { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string? Description { get; set; }
    public int? IdCatalogueHigher { get; set; }
    public int Orden { get; set; }
}


public class CatalogueCreateRequest  : CatalogueBaseRequest
{
    public bool IsActive { get => true; }
}

public class CatalogueUpdateRequest  : CatalogueBaseRequest
{
    public bool IsActive { get; set; }
}