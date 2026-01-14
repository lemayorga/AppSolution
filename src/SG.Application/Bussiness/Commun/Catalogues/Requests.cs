namespace SG.Application.Bussiness.Commun.Catalogues.Requests;

public abstract class CatalogueBaseRequest
{
    public string Group { get; set; } = default!;
    public string Value { get; set; } = default!;
    public string? Description { get; set; }
}


public class CatalogueCreateRequest  : CatalogueBaseRequest
{
    public CatalogueCreateRequest() { }
    public bool IsActive { get => true; }
}

public class CatalogueUpdateRequest  : CatalogueBaseRequest
{
    public bool IsActive { get; set; }
}