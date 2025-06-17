
using SG.Application.Base.ServiceLogic;
using SG.Application.Bussiness.Commun.Catalogues.Requests;
using SG.Application.Bussiness.Commun.Catalogues.Responses;
using SG.Domain.Entities.Commun;

namespace SG.Application.Bussiness.Commun.Catalogues.Interfaces;

public interface ICatalogueService : IServices, IBaseGenericService<Catalogue, CatalogueResponse, CatalogueCreateRequest, CatalogueUpdateRequest>
{
    //Task<PagedList<IEnumerable<CatalogueResponse>>> Paginate(int pageNumber,int pageSize, string? searchTerm, Dictionary<string, Dictionary<string, string>>? columns);
}