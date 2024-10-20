using FluentResults;
using SG.Application.Responses;
using SG.Domain;

namespace SG.Application.Bussiness;

public interface  IBaseGenericService<TEntity, TDtoRecord, TDtoCreate, TDtoUpdate>
    where TEntity : class , IEntity
    where TDtoRecord : class
    where TDtoCreate : class
    where TDtoUpdate : class
{
    Task<Result<IEnumerable<TDtoRecord>>> GetAll();
    Task<Result<TDtoRecord>> GetById(int id);
    Task<Result<TDtoRecord>> AddSave(TDtoCreate dto);
    Task<Result<bool>> DeleteById(int id);
    Task<Result<TDtoRecord>> UpdateById(int id, TDtoUpdate dto);
}
