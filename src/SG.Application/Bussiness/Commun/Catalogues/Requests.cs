using SG.Shared.Request;

namespace SG.Application.Bussiness.Commun.Catalogues.Requests;

public abstract class CatalogueBaseRequest
{
    public string Group { get; set; } = default!;
    public string Value { get; set; } = default!;
    public string? Description { get; set; }
}


public class CatalogueCreateRequest  : CatalogueBaseRequest
{
    public bool IsActive { get => true; }
}

public class CatalogueUpdateRequest  : CatalogueBaseRequest, IBaseWithIdRequest<int>
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
}
