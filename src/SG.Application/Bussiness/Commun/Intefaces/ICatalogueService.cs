
using SG.Application.Bussiness.Commun.Dtos;
using SG.Application.Responses;
using SG.Domain.Commun.Entities;

namespace SG.Application.Bussiness.Commun.Intefaces;

public interface ICatalogueService : IBaseGenericService<Catalogue, CatalogueDto, CatalogueCreateDto, CatalogueUpdateDto>
{
    Task<PagedList<IEnumerable<CatalogueDto>>> Paginate(int pageNumber,int pageSize, string? searchTerm, Dictionary<string, Dictionary<string, string>>? columns);
}