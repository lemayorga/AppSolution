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
    /// Actualizar campos de un nuevo registro por su Id
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

    /// <summary>
    /// Agrega usuarios a un rol
    /// </summary>
    /// <param name="idRole">Id del rol</param>  
    /// <param name="listIdUsers">Objeto con los datos id usuarios a insertar</param>    
    [HttpPost("AddUsersToRoleById/{idRole}")]
    [ProducesResponseType(typeof(OperationResult<bool>), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddUsersToRole(int idRole, [FromBody] List<int> listIdUsers)
    {
        var response = await _application.AddUsersToRole(idRole, listIdUsers);
        return Ok(response.ToOperationResult());
    }

    /// <summary>
    /// Agrega usuarios a un rol
    /// </summary>
    /// <param name="codeRole">Id del rol</param>  
    /// <param name="listIdUsers">Objeto con los datos id usuarios a insertar</param>    
    [HttpPost("AddUsersToRoleByCode/{codeRole}")]
    [ProducesResponseType(typeof(OperationResult<bool>), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddUsersToRole(string codeRole, [FromBody] List<int> listIdUsers)
    {
        var response = await _application.AddUsersToRole(codeRole, listIdUsers);
        return Ok(response.ToOperationResult());
    }

    /// <summary>
    /// Obtener los datos de usuarios y roles asociados
    /// </summary>
    /// <param name="filters"></param>
    /// <returns></returns>
    [HttpPost("usersWithRoles")]
    [ProducesResponseType(typeof(OperationResult<IEnumerable<UserRolesDto>>), StatusCodes.Status201Created)]
    public async Task<IActionResult> GetUsersWithRoles([FromBody] FilterUsersRoles filters)
    {
        var response = await _application.GetFilterUsersAndRoles(filters);
        return Ok(response.ToOperationResult());
    }
}