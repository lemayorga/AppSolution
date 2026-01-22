using AutoMapper;
using Microsoft.Extensions.Logging;
using SG.Application.Base.ServiceLogic;
using SG.Application.Bussiness.Commun.Catalogues.Interfaces;
using SG.Application.Bussiness.Commun.Catalogues.Requests;
using SG.Application.Bussiness.Commun.Catalogues.Responses;
using SG.Application.Base.Responses;
using SG.Domain.Base;
using SG.Domain.Entities.Commun;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using FluentResults;

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

     public async Task<Result<IEnumerable<CatalogueWithChildrenResponse>>> GetCatalogueResultChildren(Expression<Func<Catalogue, bool>>? where = null)
     {
          var list =  _unitOfWork.CatalogueRepository.GetAll(where)
                              .Include(e => e.CatalogueChildren)
                              .Where(f => !f.IdCatalogueHigher.HasValue)
                              .AsSplitQuery();

          CatalogueWithChildrenResponse convertToResult(Catalogue s, int level) => new CatalogueWithChildrenResponse
          {
               Id = s.Id,
               Value = s.Value,
               Code = s.Code,
               IsActive = s.IsActive,
               IdCatalogueHigher = s.IdCatalogueHigher,
               Description = s.Description,
               Level = level,
               Children = s.CatalogueChildren.Select(c => convertToResult(c, (level + 1)))
          };

          var result = new List<CatalogueWithChildrenResponse>();
          for (int i = 0; i < list.Count(); i++)
          {
               var elem =  list.ElementAt(0);
               var item =  convertToResult(elem, 0);
               result.Add(item);
          }

          return await Task.FromResult(Result.Ok(result.AsEnumerable()));
     }
}