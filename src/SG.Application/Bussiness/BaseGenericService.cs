
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SG.Domain;

namespace SG.Application.Bussiness;

public   class BaseGenericService<TEntity, TDto> : IBaseGenericService<TEntity, TDto>
    where TEntity : class
    where TDto : class
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

    public virtual async Task<ResultGeneric<IEnumerable<TDto>>> GetAll()
    {
        try 
        {
            var result = await _unitOfWork.Repository<TEntity>().GetAll().ToListAsync();
            return ResultGeneric<IEnumerable<TDto>>.Ok(_mapper.Map<IEnumerable<TDto>>(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ECXCEPTION_MESSAGE, ex.Message);
            return ResultGeneric<IEnumerable<TDto>>.Failure(ex.Message);
        }
    }

    public virtual async Task<ResultGeneric<TDto>> GetById(int id)
    {
        try 
        {
            var result =await _unitOfWork.Repository<TEntity>().GetById(id);
            return ResultGeneric<TDto>.Ok(_mapper.Map<TDto>(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ECXCEPTION_MESSAGE, ex.Message);
            return ResultGeneric<TDto>.Failure(ex.Message);
        }        
    }    

    public virtual async Task<ResultGeneric<TDto>> AddSave(TDto dto)
    {
        try 
        {
            var result =  await _unitOfWork.Repository<TEntity>().AddSave(_mapper.Map<TEntity>(dto));
            return ResultGeneric<TDto>.Ok(_mapper.Map<TDto>(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ECXCEPTION_MESSAGE, ex.Message);
            return ResultGeneric<TDto>.Failure(ex.Message);
        }         
    }

    public virtual async Task<ResultGeneric<bool>> DeleteById(int id)
    {
        try 
        {
            await _unitOfWork.Repository<TEntity>().DeleteById(id);
            var result = await _unitOfWork.SaveChangesAsync();
            return ResultGeneric<bool>.Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ECXCEPTION_MESSAGE, ex.Message);
            return ResultGeneric<bool>.Failure(ex.Message);
        }         
    }

    public virtual async Task<ResultGeneric<TDto>> UpdateById(int id, TDto dto)
    {
        try 
        {
            var entity = _mapper.Map<TEntity>(dto);
            var result =await _unitOfWork.Repository<TEntity>().UpdateByIdSave(id, entity);
            return ResultGeneric<TDto>.Ok(_mapper.Map<TDto>(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ECXCEPTION_MESSAGE, ex.Message);
            return ResultGeneric<TDto>.Failure(ex.Message);
        }         
    }
}
