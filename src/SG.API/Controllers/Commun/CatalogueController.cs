using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SG.Application.Bussiness.Commun.Catalogues.Interfaces;
using SG.Application.Bussiness.Commun.Catalogues.Requests;
using SG.Application.Bussiness.Commun.Catalogues.Responses;
using SG.Application.Base.Responses;
using SG.Shared.Responses;
using SG.Application.Extensions;
using SG.Application.Base.Validations;
using SG.Infrastructure.Base.Pagination;
using SG.Application.Base.Pagination;

namespace  SG.API.Controllers.Commun;

[ApiController]
[Route("api/[controller]")]
public class CatalogueController(ICatalogueService application, IDynamicValidator validator) : BaseController<CatalogueResponse, CatalogueCreateRequest, CatalogueUpdateRequest>
{
    private readonly ICatalogueService _application = application;
    private readonly IDynamicValidator _validator = validator;

    /// <summary>
    /// Obtener todos los registros.
    /// </summary> 
    /// <returns>Retornar todos los regisrtos.</returns>
    [HttpGet("")]
    [ProducesResponseType(typeof(OperationResult<IEnumerable<CatalogueResponse>>), StatusCodes.Status200OK)]
    public override async Task<IActionResult> Get()
    {
        var response = await _application.GetAll();
        return Ok(response.ToOperationResult());
    }

    /// <summary>
    /// Obtener un registro  por Id
    /// </summary>
    /// <param name="id" example="1">id del registro</param>    
    /// <returns>Retornar el registro.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(OperationResult<CatalogueResponse>), StatusCodes.Status200OK)]
    public override async Task<IActionResult> Get(int id)
    {
        var response = await _application.GetById(id);
        return Ok(response.ToOperationResult());
    }

    /// <summary>
    /// Obtener registros por lista de Ids
    /// </summary>
    /// <param name="listIds">ids de los registros</param>    
    /// <returns>Retornar el registro.</returns>
    [HttpGet("getByListIds")]
    [ProducesResponseType(typeof(OperationResult<List<CatalogueResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery(Name = "ids"), BindRequired] List<int> listIds)
    {
        var response = await _application.GetByListIds(listIds);
        return Ok(response.ToOperationResult());
    }

    /// <summary>
    /// Eliminar un registro por Id
    /// </summary>
    /// <param name="id" example="1">id del registro</param>   
    /// <returns>Retornar si fue exitoso la eliminación.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(OperationResult<bool>), StatusCodes.Status200OK)]
    public override async Task<IActionResult> Delete(int id)
    {
        var response = await _application.DeleteById(id);
        return Ok(response.ToOperationResult());
    }

    /// <summary>
    /// Agrega un nuevo registro
    /// </summary>
    /// <param name="request">Objeto con los datos a insertar</param>    
    [HttpPost("")]
    [ProducesResponseType(typeof(OperationResult<SuccessWithIdResponse>), StatusCodes.Status201Created)]
    public override async Task<IActionResult> Post([FromBody] CatalogueCreateRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Ok(validationResult.ToOperationResultErrors<SuccessWithIdResponse>());
        }

        var response = await _application.AddSave(request);
        return Ok(response.ToOperationResult());
    }

    /// <summary>
    /// Agrega lista de nuevos registros
    /// </summary>
    /// <param name="request">Objeto con los datos a insertar</param>    
    [HttpPost("addMany")]
    [ProducesResponseType(typeof(OperationResult<List<SuccessWithIdResponse>>), StatusCodes.Status201Created)]
    public async Task<IActionResult> PostMany([FromBody] List<CatalogueCreateRequest> request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Ok(validationResult.ToOperationResultErrors<SuccessWithIdResponse>());
        }
        var response = await _application.AddManySave(request);
        return Ok(response.ToOperationResult());
    }

    /// <summary>
    /// Acctualizar campos de un nuevo registro por su Id
    /// </summary>
    /// <param name="id" example="1">id del registro</param>  
    /// <param name="request">Objeto con los datos a insertar</param>    
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(OperationResult<SuccessWithIdResponse>), StatusCodes.Status200OK)]
    public override async Task<IActionResult> Put(int id, [FromBody] CatalogueUpdateRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Ok(validationResult.ToOperationResultErrors<SuccessWithIdResponse>());
        }
        var response = await _application.UpdateById(id, request);
        return Ok(response.ToOperationResult());
    }

    // /// <summary>
    // /// List paginate data
    // /// </summary>
    // /// <param name="request">Pagination parameters</param>  
    // /// <param name="columns" example="&#123;&quot;order&quot;: &#123;  &quot;Key1&quot;: &quot;Asc&quot; , &quot;Key2&quot;: &quot;Asc&quot;   &#125; , &quot;filters&quot; : &#123; &quot;Key3&quot;: &quot;Value&quot;,  &quot;Key4&quot;: &quot;Value&quot;  &#125; &#125;"></param>
    // /// <remarks>
    // /// The dictionary parameter columns should contain key-value pairs where:
    // /// - **Key**: The name of the column entity (e.g., "Status", "Category").
    // /// - **Value**: The value to filter by (e.g., "active", "electronics").
    // /// - **Asc**: The order of the column entity (e.g., "true", "false").
    // /// Example:   { "order": {  "Key": "Asc", "Key": "Asc" },  "filters": { "Key": "Value",  "Key": "Value"  } }
    // /// </remarks>
    // /// <returns> This endpoint returns a list of items.</returns>    
    // [HttpGet("paginate")]   
    // [ProducesResponseType(typeof(PagedList<List<CatalogueDto>>), StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(PagedList<object>), StatusCodes.Status404NotFound)]    
    // public async Task<IActionResult> Paginate([FromQuery] PaginationRequest request, [FromQuery] Dictionary<string, Dictionary<string, string>>? columns)
    // {   
    //     var response =await _application.Paginate(request.PageNumber, request.PageSize, request.SearchTerm , columns);
    //     if (response.IsSuccess)  return Ok(response);

    //     return BadRequest(response.Message);
    // }
    
    [HttpGet("paginate")]   
    // [ProducesResponseType(typeof(PagedList<List<CatalogueDto>>), StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(PagedList<object>), StatusCodes.Status404NotFound)]    
    public async Task<IActionResult> Paginate([FromQuery, BindRequired] PaginationRequest request,  [FromQuery] FilterParam[] filters)
    {   
        var response =await _application.GetPagination<CatalogueResponse>(request.GetParameters(), filters);
        if (response.IsSuccess)  return Ok(response);

        return BadRequest(response.Message);
    }
}