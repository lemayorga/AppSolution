using Microsoft.AspNetCore.Mvc;
using SG.Application.Bussiness.Security.Users.Interfaces;
using SG.Application.Bussiness.Security.Users.Requests;
using SG.Application.Bussiness.Security.Users.Responses;
using SG.Application.Responses;
using SG.Application.Extensions;
using SG.Shared.Responses;

namespace SG.API.Controllers.Security;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService application)  : BaseController<UserResponse, UserCreateRequest, UserUpdateRequest>
{
    private readonly IUserService _application  = application;
      
    /// <summary>
    /// Obtener todos los registros.
    /// </summary> 
    /// <returns>Retornar todos los regisrtos.</returns>
    [HttpGet("")]
    [ProducesResponseType(typeof(OperationResult<IEnumerable<UserResponse>>), StatusCodes.Status200OK)]   
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
    [ProducesResponseType(typeof(OperationResult<UserResponse>), StatusCodes.Status200OK)]
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
    [ProducesResponseType(typeof(OperationResult<SuccessWithIdResponse>), StatusCodes.Status201Created)]
    public override async Task<IActionResult> Post([FromBody] UserCreateRequest request)
    {
        var response = await _application.AddSave(request);
        return Ok(response.ToOperationResult());
    }

    /// <summary>
    /// Agrega lista de nuevos registros
    /// </summary>
    /// <param name="request">Objeto con los datos a insertar</param>    
    [HttpPost("addMany")]
    [ProducesResponseType(typeof(OperationResult<List<SuccessWithIdResponse>>), StatusCodes.Status201Created)]
    public async Task<IActionResult> PostMany([FromBody] List<UserCreateRequest> request)
    {
        var response = await _application.AddManySave(request);
        return Ok(response.ToOperationResult());
    }

    /// <summary>
    /// Actualizar campos de un nuevo registro por su Id
    /// </summary>
    /// <param name="id" example="1">id del registro</param>  
    /// <param name="request">Objeto con los datos a insertar</param>    
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(OperationResult<SuccessWithIdResponse>), StatusCodes.Status200OK)]    
    public override async Task<IActionResult> Put(int id,[FromBody] UserUpdateRequest request)
    {
        var response = await _application.UpdateById(id, request);
        return Ok(response.ToOperationResult());
    }

    /// <summary>
    /// Actualizar contrase;an del usuario
    /// </summary>
    /// <param name="request">Objeto con los datos utilizar</param>    
    [HttpPut("changePassword")]
    [ProducesResponseType(typeof(OperationResult<bool>), StatusCodes.Status200OK)]    
    public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordRequest request)
    {
        var response = await _application.ChangePassword(request);
        return Ok(response.ToOperationResult());
    }  

    /// <summary>
    /// Resetear contrase;an del usuario
    /// </summary>
    /// <param name="request">Objeto con los datos utilizar</param>    
    [HttpPut("resetPassword")]
    [ProducesResponseType(typeof(OperationResult<bool>), StatusCodes.Status200OK)]    
    public async Task<IActionResult> ResetPassword([FromBody] UserResetPasswordRequest request)
    {
        var response = await _application.ResetPassword(request);
        return Ok(response.ToOperationResult());
    }      

    /// <summary>
    /// Resetear contrase;an por id usuario
    /// </summary>
    /// <param name="idUser" example="1">id del usuario</param>      
    [HttpPut("resetPasswordBydIdUser/{idUser:int}")]
    [ProducesResponseType(typeof(OperationResult<bool>), StatusCodes.Status200OK)]    
    public async Task<IActionResult> ResetPasswordBydIdUser(int idUser)
    {
        var response = await _application.ResetPasswordBydIdUser(idUser);
        return Ok(response.ToOperationResult());
    } 

    /// <summary>
    /// Actualizar estado bloqueado
    /// </summary>
    /// <param name="idUser" example="1">id del usuario</param>  
    /// <param name="newStatusLocked">nuevo  valor a aplicar</param>     
    [HttpPut("updateStatusIsLocked/{idUser:int}")]
    [ProducesResponseType(typeof(OperationResult<bool>), StatusCodes.Status200OK)]    
    public async Task<IActionResult> UpdateStatusIsLocked(int idUser,[FromQuery(Name = "status")] bool newStatusLocked)
    {
        var response = await _application.UpdateStatusIsLocked(idUser, newStatusLocked);
        return Ok(response.ToOperationResult());
    } 

    /// <summary>
    /// Actualizar estado activo
    /// </summary>
    /// <param name="idUser" example="1">id del usuario</param>  
    /// <param name="newStatusActive">nuevo  valor a aplicar</param>     
    [HttpPut("updateStatusIsActived/{idUser:int}")]
    [ProducesResponseType(typeof(OperationResult<bool>), StatusCodes.Status200OK)]    
    public async Task<IActionResult> UpdateStatusIsActived(int idUser,[FromQuery(Name = "status")] bool newStatusActive)
    {
        var response = await _application.UpdateStatusIsActived(idUser, newStatusActive);
        return Ok(response.ToOperationResult());
    } 
}