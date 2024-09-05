using SG.Application.Responses;
using SG.Domain;

namespace SG.Application.Bussiness;

public interface  IBaseGenericService<TEntity, TDtoRecord, TDtoCreate, TDtoUpdate>
    where TEntity : class , IEntity
    where TDtoRecord : class
    where TDtoCreate : class
    where TDtoUpdate : class
{
    Task<ResultGeneric<IEnumerable<TDtoRecord>>> GetAll();
    Task<ResultGeneric<TDtoRecord>> GetById(int id);
    Task<ResultGeneric<TDtoRecord>> AddSave(TDtoCreate dto);
    Task<ResultGeneric<bool>> DeleteById(int id);
    Task<ResultGeneric<TDtoRecord>> UpdateById(int id, TDtoUpdate dto);
}
