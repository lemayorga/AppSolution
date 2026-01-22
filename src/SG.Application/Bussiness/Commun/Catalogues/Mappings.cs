using SG.Application.Bussiness.Commun.Catalogues.Requests;
using SG.Application.Bussiness.Commun.Catalogues.Responses;
using SG.Domain.Entities.Commun;

namespace SG.Application.Bussiness.Commun.Catalogues;

public static class Mappings
{
    public static CatalogueResponse ToDto(this Catalogue model)
    {
        return new CatalogueResponse
        {
            Id = model.Id,
            Value = model.Value,
            Code = model.Code,
            IsActive = model.IsActive,
            IdCatalogueHigher = model.IdCatalogueHigher,
            Description = model.Description,
        };
    }

    public static IEnumerable<CatalogueResponse> ToDtos(this IEnumerable<Catalogue> models)
    {
        return models.Select(p => p.ToDto());
    }

    public static Catalogue ToModel(this CatalogueCreateRequest dto)
    {
        return new Catalogue
        {
            Value = dto.Value,
            Code = dto.Code,
            IsActive = dto.IsActive,
            IdCatalogueHigher = dto.IdCatalogueHigher,
            Description = dto.Description,
        };
    }

    public static Catalogue ToModel(this CatalogueUpdateRequest dto)
    {
        return new Catalogue
        {
            Value = dto.Value,
            Code = dto.Code,
            IsActive = dto.IsActive,
            IdCatalogueHigher = dto.IdCatalogueHigher,
            Description = dto.Description,
        };
    }
}
