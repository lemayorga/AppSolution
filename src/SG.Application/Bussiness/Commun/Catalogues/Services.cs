using AutoMapper;
using Microsoft.Extensions.Logging;
using SG.Application.Base.ServiceLogic;
using SG.Application.Bussiness.Commun.Catalogues.Interfaces;
using SG.Application.Bussiness.Commun.Catalogues.Requests;
using SG.Application.Bussiness.Commun.Catalogues.Responses;
using SG.Domain;

namespace SG.Application.Bussiness.Commun.Catalogues.Services;


public class CatalogueService : BaseGenericService<Domain.Commun.Entities.Catalogue, CatalogueResponse, CatalogueCreateRequest, CatalogueUpdateRequest>,  ICatalogueService
{
    public CatalogueService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CatalogueService> logger) : base(unitOfWork, mapper, logger){ }

   /* public override async Task<PagedList<IEnumerable<CatalogueDto>>> Paginate(int pageNumber,int pageSize, string? searchTerm, Dictionary<string, Dictionary<string, string>>? columns)
    {
        try 
        {

           var (filters, orders) = this.GetDataDictionaryColumnsByPaginate(columns);
           var(total, data) =  await _unitOfWork.CatalogueRepository.Paginate(pageNumber, pageSize, searchTerm, filters, orders);
           var modelMapper = _mapper.Map<IEnumerable<CatalogueDto>>(data);
            var pagedList = new PagedList<IEnumerable<CatalogueDto>>(modelMapper, total, pageNumber, pageSize);                                    
            return pagedList;
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, ECXCEPTION_MESSAGE, ex.Message);
            return new PagedList<IEnumerable<CatalogueDto>>([], 0, pageNumber, pageSize, ex, ex.Message);     
        }
    }   */ 
}