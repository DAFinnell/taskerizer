using Microsoft.AspNetCore.Mvc;

namespace Taskerizer.Controllers;

[ApiController]
[Route("/health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public ActionResult<String> GetHealth()
    {
        return Ok("Service is correctly operating");
    }
}