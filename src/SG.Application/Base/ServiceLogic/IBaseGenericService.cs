using FluentResults;
using SG.Domain.Base;
using SG.Shared.Responses;

namespace SG.Application.Base.ServiceLogic;

public interface  IBaseGenericService<TEntity, TDtoRecord, TDtoCreate, TDtoUpdate>
    where TEntity : BaseEntity<int>  //class , IEntity
    where TDtoRecord : class
    where TDtoCreate : class
    where TDtoUpdate : class
{
    Task<Result<IEnumerable<TDtoRecord>>> GetAll();
    Task<Result<TDtoRecord>> GetById(int id);
    Task<Result<List<TDtoRecord>>> GetByListIds(List<int> listIds);
    Task<Result<SuccessWithIdResponse>> AddSave(TDtoCreate dto);
    Task<Result<List<SuccessWithIdResponse>>> AddManySave(List<TDtoCreate> modelDto);
    Task<Result<bool>> DeleteById(int id);
    Task<Result<SuccessWithIdResponse>> UpdateById(int id, TDtoUpdate dto);
}
