
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SG.Application.Responses;
using SG.Domain;
using SG.Shared.Constants;

namespace SG.Application.Bussiness;

public   class BaseGenericService<TEntity, TDtoRecord, TDtoCreate, TDtoUpdate> : IBaseGenericService<TEntity, TDtoRecord, TDtoCreate, TDtoUpdate>
    where TEntity : class , IEntity
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

    public virtual async Task<ResultGeneric<IEnumerable<TDtoRecord>>> GetAll()
    {
        try 
        {
            var result = await _unitOfWork.Repository<TEntity>().GetAll().ToListAsync();
            return ResultGeneric<IEnumerable<TDtoRecord>>.Ok(_mapper.Map<IEnumerable<TDtoRecord>>(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ECXCEPTION_MESSAGE, ex.Message);
            return ResultGeneric<IEnumerable<TDtoRecord>>.Failure(ex.Message);
        }
    }

    public virtual async Task<ResultGeneric<TDtoRecord>> GetById(int id)
    {
        try 
        {
            var result = await _unitOfWork.Repository<TEntity>().GetById(id);
            if(result == null)
            {
                 return ResultGeneric<TDtoRecord>.Failure(Constantes.NOT_ITEM_FOUND_DATABASE);
            }
            return ResultGeneric<TDtoRecord>.Ok(_mapper.Map<TDtoRecord>(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ECXCEPTION_MESSAGE, ex.Message);
            return ResultGeneric<TDtoRecord>.Failure(ex.Message);
        }        
    }    

    public virtual async Task<ResultGeneric<TDtoRecord>> AddSave(TDtoCreate dto)
    {
        try 
        {
            var result =  await _unitOfWork.Repository<TEntity>().AddSave(_mapper.Map<TEntity>(dto));
            return ResultGeneric<TDtoRecord>.Ok(_mapper.Map<TDtoRecord>(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ECXCEPTION_MESSAGE, ex.Message);
            return ResultGeneric<TDtoRecord>.Failure(ex.Message);
        }         
    }

    public virtual async Task<ResultGeneric<bool>> DeleteById(int id)
    {
        try 
        {
             var result = await _unitOfWork.Repository<TEntity>().DeleteByIdSave(id);
            if(!result)
            {
                return ResultGeneric<bool>.Failure(Constantes.NOT_ITEM_FOUND_DATABASE);
            }

            return ResultGeneric<bool>.Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ECXCEPTION_MESSAGE, ex.Message);
            return ResultGeneric<bool>.Failure(ex.Message);
        }         
    }

    public virtual async Task<ResultGeneric<TDtoRecord>> UpdateById(int id, TDtoUpdate dto)
    {
        try 
        {
            var entity = _mapper.Map<TEntity>(dto);
            entity.Id  = entity.Id == 0 ?  id : entity.Id;
            var result = await _unitOfWork.Repository<TEntity>().UpdateByIdSave(id, entity);
            if(result == null)
            {
                 return ResultGeneric<TDtoRecord>.Failure(Constantes.NOT_ITEM_FOUND_DATABASE);
            }            
            return ResultGeneric<TDtoRecord>.Ok(_mapper.Map<TDtoRecord>(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ECXCEPTION_MESSAGE, ex.Message);
            return ResultGeneric<TDtoRecord>.Failure(ex.Message);
        }         
    }   

    public virtual async Task<PagedList<IEnumerable<TDtoRecord>>> Paginate(int pageNumber,int pageSize, string? searchTerm, Dictionary<string, Dictionary<string, string>>? columns)
    {
        try 
        {
            var (filters, orders) = GetDataDictionaryColumnsByPaginate(columns);
            var(total, data) =  await _unitOfWork.Repository<TEntity>().Paginate(pageNumber, pageSize, searchTerm, filters, orders);
            var modelMapper = _mapper.Map<IEnumerable<TDtoRecord>>(data);

            var pagedList = new PagedList<IEnumerable<TDtoRecord>>(modelMapper, total, pageNumber, pageSize);                                    
            return pagedList;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ECXCEPTION_MESSAGE, ex.Message);
            return new PagedList<IEnumerable<TDtoRecord>>([], 0, pageNumber, pageSize, ex, ex.Message);     
        }
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