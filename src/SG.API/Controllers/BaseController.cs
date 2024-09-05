using Microsoft.AspNetCore.Mvc;

namespace SG.API.Controllers;

public abstract class BaseController<TDtoRecord, TDtoCreate, TDtoUpdate> : ControllerBase
    where TDtoRecord : class
    where TDtoCreate : class
    where TDtoUpdate : class
{
    public abstract  Task<IActionResult> Get();
    public abstract  Task<IActionResult> Get(int id);
    public abstract  Task<IActionResult> Post(TDtoCreate request);
    public abstract  Task<IActionResult> Put(int id, TDtoUpdate request);
    public abstract  Task<IActionResult> Delete(int id);      
}