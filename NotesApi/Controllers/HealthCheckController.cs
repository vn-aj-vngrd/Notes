using Microsoft.AspNetCore.Mvc;

namespace NotesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HealthCheckController : ControllerBase
{
    // GET: api/HealthCheck
    [HttpGet]
    public ActionResult GetHealthCheck()
    {
        return Ok("Healthy");
    }
}
