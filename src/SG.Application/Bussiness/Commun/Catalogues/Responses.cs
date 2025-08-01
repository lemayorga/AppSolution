using SG.Domain.Entities.Commun;
using SG.Shared.Responses;

namespace SG.Application.Bussiness.Commun.Catalogues.Responses;
public  class CatalogueResponse : BaseWithIdResponse
{
    public CatalogueResponse(){}
    public CatalogueResponse
    (
        int id,
        string group,
        string value,
        bool isActive,
        string? description
    )
    {
        Id = id;
        Group = group;
        Value = value;
        IsActive = isActive;
        Description = description;
    }

    public CatalogueResponse(Catalogue model):this
    (
        id: model.Id,
        group: model.Group,
        value: model.Value,
        isActive: model.IsActive,
        description: model.Description
    ) { }

    public string Group { get; set; } = default!;
    public string Value { get; set; }  = default!;
    public bool IsActive { get; set; }
    public string? Description  { get; set; }
}