using Microsoft.AspNetCore.Mvc;
using SG.Application.Bussiness.Commun.Intefaces;

namespace SG.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICatalogueService _application;
    public HomeController(ILogger<HomeController> logger,ICatalogueService application) 
    {
        _logger = logger;
        _application = application;
    }
        
    [HttpGet("")]
    public IActionResult Home() 
    {
        _logger.LogInformation("Home method called: ");
        return Ok("Hello!");
    }

    [HttpGet("Post")]
    public async Task<IActionResult> Post() 
    {
        var d = await _application.AddSave(new Application.Bussiness.Commun.Dtos.CatalogueDto{
            Group = "Uno",
            Value = "Uno",
            IsActive = true,
            Description ="Prueba"
        });
        var dd = await _application.GetAll();

        return Ok(dd);
    }
}
