using Microsoft.AspNetCore.Mvc;

namespace TwitchDownloaderAPI.Controllers;

[Route("/health")]
[ApiExplorerSettings(IgnoreApi = true)]
[ApiController]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult GetHealth()
    {
        return Ok();
    }
}