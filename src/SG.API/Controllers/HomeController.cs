using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SG.API.Controllers;

[AllowAnonymous]
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
        return Ok("Hello!");
    }
}