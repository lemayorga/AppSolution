using Microsoft.AspNetCore.Mvc;
using SG.Application.Bussiness.Security.Dtos;
using SG.Application.Bussiness.Security.Interfaces;
using SG.Application.Responses;

namespace SG.API.Controllers.Security;

[ApiController]
[Route("api/[controller]")]
public class RoleController : BaseController<RoleDto, RoleCreateDto, RoleUpdateDto>
{
    private readonly IRoleService _application;
    public RoleController(IRoleService application) 
    {
        _application = application;
    }
      
    /// <summary>
    /// Obtener todos los registros.
    /// </summary> 
    /// <returns>Retornar todos los regisrtos.</returns>
    [HttpGet("")]
    [ProducesResponseType(typeof(OperationResult<IEnumerable<RoleDto>>), StatusCodes.Status200OK)]   
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
    [ProducesResponseType(typeof(OperationResult<RoleDto>), StatusCodes.Status200OK)]
    public override async Task<IActionResult> Get(int id)
    {
        var response = await _application.GetById(id);
        return Ok(response.ToOperationResult());
    }


    /// <summary>
    /// Eliminar un registro por Id
    /// </summary>
    /// <param name="id" example="1">id del registro</param>   
    /// <returns>Retornar si fue exitoso la eliminación.</returns>
    [HttpDelete("")]
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
    [ProducesResponseType(typeof(OperationResult<RoleDto>), StatusCodes.Status201Created)]
    public override async Task<IActionResult> Post([FromBody] RoleCreateDto request)
    {
        var response = await _application.AddSave(request);
        return Ok(response.ToOperationResult());
    }

    /// <summary>
    /// Acctualizar campos de un nuevo registro por su Id
    /// </summary>
    /// <param name="id" example="1">id del registro</param>  
    /// <param name="request">Objeto con los datos a insertar</param>    
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(OperationResult<RoleDto>), StatusCodes.Status200OK)]    
    public override async Task<IActionResult> Put(int id,[FromBody] RoleUpdateDto request)
    {
        var response = await _application.UpdateById(id, request);
        return Ok(response.ToOperationResult());
    }
}