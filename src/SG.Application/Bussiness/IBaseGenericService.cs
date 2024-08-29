namespace SG.Application.Bussiness;

public interface IBaseGenericService<TEntity, TDto> 
    where TEntity : class 
    where TDto : class
{
    Task<ResultGeneric<IEnumerable<TDto>>> GetAll();
    Task<ResultGeneric<TDto>> GetById(int id);
    Task<ResultGeneric<TDto>> AddSave(TDto dto);
    Task<ResultGeneric<bool>> DeleteById(int id);
    Task<ResultGeneric<TDto>> UpdateById(int id, TDto dto);
}
