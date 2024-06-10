using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TwitchDownloaderAPI.Controllers;

[Route("/error")]
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    private readonly ILogger<ErrorController> _logger;
    private readonly IWebHostEnvironment _env;

    public ErrorController(ILogger<ErrorController> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }
    
    public IActionResult Error()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = context?.Error;
        var statusCode = exception switch
        {
            // Add more exception types later
            _ => HttpStatusCode.InternalServerError
        };

        _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception?.Message);

        if (_env.IsDevelopment() || _env.IsEnvironment("Development"))
        {
            var response = new
            {
                StatusCode = (int)statusCode,
                Message = "An unhandled exception occurred during the request.",
                Detailed = exception?.ToString()
            };
            return StatusCode((int)statusCode, response);
        }
        else
        {
            var response = new
            {
                StatusCode = (int)statusCode,
                Message = "An error occurred while processing your request."
            };
            return StatusCode((int)statusCode, response);
        }
    }
}
