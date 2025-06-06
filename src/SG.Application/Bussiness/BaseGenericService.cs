
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SG.Application.Responses;
using SG.Domain;
using SG.Domain.Base;
using SG.Shared.Constants;

namespace SG.Application.Bussiness;

public   class BaseGenericService<TEntity, TDtoRecord, TDtoCreate, TDtoUpdate> : IBaseGenericService<TEntity, TDtoRecord, TDtoCreate, TDtoUpdate>
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

    public virtual async Task<Result<IEnumerable<TDtoRecord>>> GetAll()
    {
        var result = await _unitOfWork.Repository<TEntity>().GetAll().ToListAsync();             
        return Result.Ok(_mapper.Map<IEnumerable<TDtoRecord>>(result));
    }

    public virtual async Task<Result<TDtoRecord>> GetById(int id)
    {
        var result = await _unitOfWork.Repository<TEntity>().GetById(id);
        if(result is null)
        {
            return Result.Fail(MESSAGE_CONSTANTES.NOT_ITEM_FOUND_DATABASE);
        }
        return Result.Ok(_mapper.Map<TDtoRecord>(result));   
    }    

    public virtual async Task<Result<List<TDtoRecord>>> GetByListIds(List<int> listIds)
    {
        var result = await Task.FromResult(_unitOfWork.Repository<TEntity>().FindByCondition(f => listIds.Contains(f.Id)));
        return Result.Ok(_mapper.Map<List<TDtoRecord>>(result));       
    }    

    public virtual async Task<Result<TDtoRecord>> AddSave(TDtoCreate modelDto)
    {
        var result =  await _unitOfWork.Repository<TEntity>().AddSave(_mapper.Map<TEntity>(modelDto));
        return Result.Ok(_mapper.Map<TDtoRecord>(result));         
    }

    public virtual async Task<Result<List<TDtoRecord>>> AddManySave(List<TDtoCreate> modelDto)
    {
        var result =  await _unitOfWork.Repository<TEntity>().AddManySave(_mapper.Map<List<TEntity>>(modelDto));
        return Result.Ok(_mapper.Map<List<TDtoRecord>>(result));         
    }

    public virtual async Task<Result<bool>> DeleteById(int id)
    {
        var result = await _unitOfWork.Repository<TEntity>().DeleteByIdSave(id);
        if(!result)
        {
            return Result.Fail(MESSAGE_CONSTANTES.NOT_ITEM_FOUND_DATABASE);
        }

        return Result.Ok(result);         
    }

    public virtual async Task<Result<TDtoRecord>> UpdateById(int id, TDtoUpdate modelDto)
    {
        var entity = _mapper.Map<TEntity>(modelDto);
        entity.SetIdIfIsDefaultValue(id);

        var result = await _unitOfWork.Repository<TEntity>().UpdateByIdSave(id, entity);
        if(result == null)
        {
                return Result.Fail(MESSAGE_CONSTANTES.NOT_ITEM_FOUND_DATABASE);
        }            
        return Result.Ok(_mapper.Map<TDtoRecord>(result));        
    }

    public virtual async Task<PagedList<IEnumerable<TDtoRecord>>> Paginate(int pageNumber,int pageSize, string? searchTerm, Dictionary<string, Dictionary<string, string>>? columns)
    {
        var (filters, orders) = GetDataDictionaryColumnsByPaginate(columns);
        var(total, data) =  await _unitOfWork.Repository<TEntity>().Paginate(pageNumber, pageSize, searchTerm, filters, orders);
        var modelMapper = _mapper.Map<IEnumerable<TDtoRecord>>(data);

        var pagedList = new PagedList<IEnumerable<TDtoRecord>>(modelMapper, total, pageNumber, pageSize);                                    
        return pagedList;
    }     

    protected (Dictionary<string, string>?, Dictionary<string, string>?) GetDataDictionaryColumnsByPaginate(Dictionary<string, Dictionary<string, string>>? columns)
    {
        Dictionary<string, string>? filters = null;
        Dictionary<string, string>? orders = null;
        if(columns != null)
        {
            columns.TryGetValue("filters", out filters);
            columns.TryGetValue("order", out orders);
        }
        return (filters, orders);
    }
}