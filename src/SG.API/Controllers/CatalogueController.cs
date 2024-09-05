using Microsoft.AspNetCore.Mvc;
using SG.Application.Bussiness.Commun.Dtos;
using SG.Application.Bussiness.Commun.Intefaces;
using SG.Application.Requests;
using SG.Application.Responses;

namespace SG.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogueController : BaseController<CatalogueDto, CatalogueCreateDto, CatalogueUpdateDto>
{
    private readonly ICatalogueService _application;
    public CatalogueController(ICatalogueService application) 
    {
        _application = application;
    }
      
    /// <summary>
    /// Obtener todos los registros.
    /// </summary> 
    /// <returns>Retornar todos los regisrtos.</returns>
    [HttpGet("")]
    [ProducesResponseType(typeof(ResultGeneric<IEnumerable<CatalogueDto>>), StatusCodes.Status200OK)]   
    public override async Task<IActionResult> Get()
   {
        var response = await _application.GetAll();
        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Message);
    }

    /// <summary>
    /// Obtener un registro  por Id
    /// </summary>
    /// <param name="id" example="1">id del registro</param>    
    /// <returns>Retornar el registro.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ResultGeneric<CatalogueDto>), StatusCodes.Status200OK)]
    public override async Task<IActionResult> Get(int id)
    {
        var response = await _application.GetById(id);
        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Message);
    }


    /// <summary>
    /// Eliminar un registro por Id
    /// </summary>
    /// <param name="id" example="1">id del registro</param>   
    /// <returns>Retornar si fue exitoso la eliminación.</returns>
    [HttpDelete("")]
    [ProducesResponseType(typeof(ResultGeneric<bool>), StatusCodes.Status200OK)]

    public override async Task<IActionResult> Delete(int id)
    {
        var response = await _application.DeleteById(id);
        if (response.IsSuccess)  return Ok(response);

        return BadRequest(response.Message);
    }

    /// <summary>
    /// Agrega un nuevo registro
    /// </summary>
    /// <param name="request">Objeto con los datos a insertar</param>    
    [HttpPost("")]
    [ProducesResponseType(typeof(ResultGeneric<CatalogueDto>), StatusCodes.Status201Created)]
    public override async Task<IActionResult> Post([FromBody] CatalogueCreateDto request)
    {
        var response = await _application.AddSave(request);
        if (response.IsSuccess)  return Ok(response);

        return BadRequest(response.Message);
    }

    /// <summary>
    /// Acctualizar campos de un nuevo registro por su Id
    /// </summary>
    /// <param name="id" example="1">id del registro</param>  
    /// <param name="request">Objeto con los datos a insertar</param>    
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ResultGeneric<CatalogueDto>), StatusCodes.Status200OK)]    
    public override async Task<IActionResult> Put(int id,[FromBody] CatalogueUpdateDto request)
    {
        var response = await _application.UpdateById(id, request);
        if (response.IsSuccess)  return Ok(response);

        return BadRequest(response.Message);
    }
    
    /// <summary>
    /// List paginate data
    /// </summary>
    /// <param name="request">Pagination parameters</param>  
    /// <param name="columns" example="&#123;&quot;order&quot;: &#123;  &quot;Key1&quot;: &quot;Asc&quot; , &quot;Key2&quot;: &quot;Asc&quot;   &#125; , &quot;filters&quot; : &#123; &quot;Key3&quot;: &quot;Value&quot;,  &quot;Key4&quot;: &quot;Value&quot;  &#125; &#125;"></param>
    /// <remarks>
    /// The dictionary parameter columns should contain key-value pairs where:
    /// - **Key**: The name of the column entity (e.g., "Status", "Category").
    /// - **Value**: The value to filter by (e.g., "active", "electronics").
    /// - **Asc**: The order of the column entity (e.g., "true", "false").
    /// Example:   { "order": {  "Key": "Asc", "Key": "Asc" },  "filters": { "Key": "Value",  "Key": "Value"  } }
    /// </remarks>
    /// <returns> This endpoint returns a list of items.</returns>    
    [HttpGet("paginate")]   
    [ProducesResponseType(typeof(PagedList<List<CatalogueDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PagedList<object>), StatusCodes.Status404NotFound)]    
    public async Task<IActionResult> Paginate([FromQuery] PaginationRequest request, [FromQuery] Dictionary<string, Dictionary<string, string>>? columns)
    {   
        var response =await _application.Paginate(request.PageNumber, request.PageSize, request.SearchTerm , columns);
        if (response.IsSuccess)  return Ok(response);

        return BadRequest(response.Message);
    }
}