using Microsoft.AspNetCore.Mvc;

namespace SG.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    private readonly ILogger<HomeController> _logger;
    public HomeController(ILogger<HomeController> logger) 
    {
        _logger = logger;
    }
        
    [HttpGet("")]
    public IActionResult Home() 
    {
        _logger.LogInformation("Response: {@result}","Hello");
        
        return Ok("Hello!");
    }
}