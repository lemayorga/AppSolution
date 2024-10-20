using AutoMapper;
using SG.Domain.Commun.Entities;
using SG.Application.Bussiness.Commun.Dtos;
using SG.Application.Bussiness.Commun.Intefaces;
using Microsoft.Extensions.Logging;
using SG.Domain;
using SG.Application.Responses;

namespace SG.Application.Bussiness.Commun.Services;

public class CatalogueService : BaseGenericService<Catalogue, CatalogueDto, CatalogueCreateDto, CatalogueUpdateDto>,  ICatalogueService
{
    public CatalogueService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CatalogueService> logger) : base(unitOfWork, mapper, logger){ }

    public override async Task<PagedList<IEnumerable<CatalogueDto>>> Paginate(int pageNumber,int pageSize, string? searchTerm, Dictionary<string, Dictionary<string, string>>? columns)
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
    }    
}