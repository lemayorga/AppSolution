using FluentResults;
using SG.Application.Base.Responses;
using SG.Domain.Base;
using SG.Infrastructure.Base.Pagination;
using SG.Shared.Responses;

namespace SG.Application.Base.ServiceLogic;

/// <summary>
/// Generic interface for CRUD (Create, Read, Update, Delete) services that operate with TEntity type entities and specific DTOs for each operation.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TDtoRecord">The type of the record DTO.</typeparam>
/// <typeparam name="TDtoCreate">The type of the create DTO.</typeparam>
/// <typeparam name="TDtoUpdate">The type of the update DTO.</typeparam>
public interface IBaseGenericService<TEntity, TDtoRecord, TDtoCreate, TDtoUpdate>
    where TEntity : BaseEntity<int>  //class , IEntity
    where TDtoRecord : class
    where TDtoCreate : class
    where TDtoUpdate : class
{
    /// <summary>
    /// Gets all records of the entity, returning a list of record DTOs.
    /// </summary>
    /// <returns></returns>
    Task<Result<IEnumerable<TDtoRecord>>> GetAll();

    /// <summary>
    /// Gets a specific record of the entity by its ID, returning the corresponding record DTO.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Result<TDtoRecord>> GetById(int id);

    /// <summary>
    /// Gets a list of records of the entity by a list of IDs, returning a list of corresponding record DTOs.
    /// </summary>
    /// <param name="listIds"></param>
    /// <returns></returns> 
    Task<Result<List<TDtoRecord>>> GetByListIds(List<int> listIds);

    /// <summary>
    /// Adds a new record of the entity using the provided create DTO, returning a response with the ID of the created record.  
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<Result<SuccessWithIdResponse>> AddSave(TDtoCreate dto);

    /// <summary>
    /// Adds multiple new records of the entity using a list of create DTOs, returning a list of responses with the IDs of the created records.
    /// </summary>
    /// <param name="modelDto"></param>
    /// <returns></returns>
    Task<Result<List<SuccessWithIdResponse>>> AddManySave(List<TDtoCreate> modelDto);

    /// <summary>
    /// Deletes a specific record of the entity by its ID, returning a boolean result indicating whether the deletion was successful.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Result<bool>> DeleteById(int id);

    /// <summary>
    ///  Updates a specific record of the entity by its ID using the provided update DTO, returning a response with the ID of the updated record.    
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<Result<SuccessWithIdResponse>> UpdateById(int id, TDtoUpdate dto);

    /// <summary>
    /// Gets a paginated list of records of the entity based on the provided pagination parameters and optional filters, returning a paginated response with a list of record DTOs.
    /// </summary>
    /// <typeparam name="TRecordPagination"></typeparam>
    /// <param name="pagination"></param>
    /// <param name="filters"></param>
    /// <returns></returns>
    Task<PagedList<IEnumerable<TRecordPagination>>> GetPagination<TRecordPagination>(PaginationParams pagination, FilterParam[]? filters = null);
}
