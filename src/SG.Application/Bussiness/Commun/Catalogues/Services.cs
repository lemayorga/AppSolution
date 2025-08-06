using AutoMapper;
using Microsoft.Extensions.Logging;
using SG.Application.Base.ServiceLogic;
using SG.Application.Bussiness.Commun.Catalogues.Interfaces;
using SG.Application.Bussiness.Commun.Catalogues.Requests;
using SG.Application.Bussiness.Commun.Catalogues.Responses;
using SG.Application.Base.Responses;
using SG.Domain.Base;
using SG.Domain.Entities.Commun;

namespace SG.Application.Bussiness.Commun.Catalogues.Services;


public class CatalogueService : BaseGenericService<Catalogue, CatalogueResponse, CatalogueCreateRequest, CatalogueUpdateRequest>,  ICatalogueService
{
    public CatalogueService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CatalogueService> logger) : base(unitOfWork, mapper, logger){ }

   public override async Task<PagedList<IEnumerable<CatalogueResponse>>> Paginate(int pageNumber,int pageSize, string? searchTerm, Dictionary<string, Dictionary<string, string>>? columns)
   {
        var (filters, orders) = this.GetDataDictionaryColumnsByPaginate(columns);
        var(total, data) =  await _unitOfWork.CatalogueRepository.Paginate(pageNumber, pageSize, searchTerm, filters, orders);
        var modelMapper = _mapper.Map<IEnumerable<CatalogueResponse>>(data);
        var pagedList = new PagedList<IEnumerable<CatalogueResponse>>(modelMapper, total, pageNumber, pageSize);                                    
        return pagedList;
   }   
}