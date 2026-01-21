using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SG.Application.Base.Responses;
using SG.Domain.Base;
using SG.Infrastructure.Base.Pagination;
using SG.Infrastructure.Data.Extensions;
using SG.Shared.Constants;
using SG.Shared.Responses;

namespace SG.Application.Base.ServiceLogic;

public class BaseGenericService<TEntity, TDtoRecord, TDtoCreate, TDtoUpdate> : IBaseGenericService<TEntity, TDtoRecord, TDtoCreate, TDtoUpdate>
    where TEntity : BaseEntity<int>  //class , IEntity
    where TDtoRecord : class
    where TDtoCreate : class
    where TDtoUpdate : class
{
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IMapper _mapper;
    protected readonly ILogger _logger;
    protected const string ECXCEPTION_MESSAGE = @"Exception occurred: {Message}";

    public BaseGenericService(IUnitOfWork unitOfWork, IMapper mapper, ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    protected internal IBaseGenericRepository<TEntity> GetInstanceRepository()
        => _unitOfWork.Repository<TEntity>();
    
    public virtual async Task<Result<IEnumerable<TDtoRecord>>> GetAll()
    {
        var result = await _unitOfWork.Repository<TEntity>().GetAll().ToListAsync();
        return Result.Ok(_mapper.Map<IEnumerable<TDtoRecord>>(result));
    }

    public virtual async Task<Result<TDtoRecord>> GetById(int id)
    {
        var result = await _unitOfWork.Repository<TEntity>().GetById(id);
        if (result is null)
        {
            return Result.Fail(MESSAGE_CONSTANTS.NOT_ITEM_FOUND_DATABASE);
        }
        return Result.Ok(_mapper.Map<TDtoRecord>(result));
    }

    public virtual async Task<Result<List<TDtoRecord>>> GetByListIds(List<int> listIds)
    {
        var result = await Task.FromResult(_unitOfWork.Repository<TEntity>().FindByCondition(f => listIds.Contains(f.Id)));
        return Result.Ok(_mapper.Map<List<TDtoRecord>>(result));
    }

    public virtual async Task<Result<SuccessWithIdResponse>> AddSave(TDtoCreate modelDto)
    {
        var model = _mapper.Map<TEntity>(modelDto);
        var repository = GetInstanceRepository();
        await repository.Add(model);
        await repository.SaveChangesAsync();
        return Result.Ok(new SuccessWithIdResponse(model.Id));
    }

    public virtual async Task<Result<List<SuccessWithIdResponse>>> AddManySave(List<TDtoCreate> modelDto)
    {
        var model = _mapper.Map<List<TEntity>>(modelDto);
        var repository = GetInstanceRepository();
        await repository.AddMany(model);
        await repository.SaveChangesAsync();
        var result = model.Select(x => new SuccessWithIdResponse(x.Id)).ToList();
        return Result.Ok(result);
    }

    public virtual async Task<Result<bool>> DeleteById(int id)
    {
        var repository = GetInstanceRepository();
        var result = await repository.DeleteById(id);
        if (!result)
        {
            return Result.Fail(MESSAGE_CONSTANTS.NOT_ITEM_FOUND_DATABASE);
        }

        result = await repository.SaveChangesAsync();
        return Result.Ok(result);
    }

    public virtual async Task<Result<SuccessWithIdResponse>> UpdateById(int id, TDtoUpdate modelDto)
    {
        var entity = _mapper.Map<TEntity>(modelDto);
        entity.SetIdIfIsDefaultValue(id);

        var repository = GetInstanceRepository();
        var result = await repository.UpdateById(id, entity);
        if (!result)
        {
            return Result.Fail(MESSAGE_CONSTANTS.NOT_ITEM_FOUND_DATABASE);
        }

        await repository.SaveChangesAsync();
        return Result.Ok(new SuccessWithIdResponse(id));
    }

    public virtual async Task<PagedList<IEnumerable<TDtoRecord>>> Paginate(int pageNumber, int pageSize, string? searchTerm, Dictionary<string, Dictionary<string, string>>? columns)
    {
        var (filters, orders) = GetDataDictionaryColumnsByPaginate(columns);
        var (total, data) = await _unitOfWork.Repository<TEntity>().Paginate(pageNumber, pageSize, searchTerm, filters, orders);
        var modelMapper = _mapper.Map<IEnumerable<TDtoRecord>>(data);

        var pagedList = new PagedList<IEnumerable<TDtoRecord>>(modelMapper, total, pageNumber, pageSize);
        return pagedList;
    }

    protected (Dictionary<string, string>?, Dictionary<string, string>?) GetDataDictionaryColumnsByPaginate(Dictionary<string, Dictionary<string, string>>? columns)
    {
        Dictionary<string, string>? filters = null;
        Dictionary<string, string>? orders = null;
        if (columns != null)
        {
            columns.TryGetValue("filters", out filters);
            columns.TryGetValue("order", out orders);
        }
        return (filters, orders);
    }
    
    public virtual List<string> GetPropertiesSearch() => new  List<string> ();
 
    public virtual async Task<PagedList<IEnumerable<TRecordPagination>>> GetPagination<TRecordPagination>(PaginationParams pagination, FilterParam[]? filters = null)
    {
        var query = _unitOfWork.Repository<TEntity>().GetAll();

        if (!string.IsNullOrWhiteSpace(pagination.SearchTerm))
        {
            // Get all public instance properties
            var properties = typeof(TEntity)
                .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .Select(p => p.Name)
                .ToList();

            query = query.ApplySearch(new SearchParameters
            {
                SearchTerm = pagination.SearchTerm,
                SearchFields = [..  GetPropertiesSearch()]
            });
        }
        query = query.ApplyFilters(filters);

        var (totalCount, queryData) = query.ApplyPagination(pagination);
        var modelMapper = _mapper.Map<IEnumerable<TRecordPagination>>(queryData);
        var result = new PagedList<IEnumerable<TRecordPagination>>(modelMapper, totalCount, pagination.PageNumber, pagination.PageSize);

        return await Task.FromResult(result);
    }    
}