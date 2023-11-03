using System.Security.Claims;
using Serilog.Context;

namespace TwitchDownloaderAPI.Middlewares;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        try
        {
            var correlationId = ctx.Request.Headers["x-correlation-id"].FirstOrDefault();
            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                // Log request details
                _logger.LogInformation("{Method} request to {Path}", ctx.Request.Method, ctx.Request.Path);

                await _next(ctx);
                
                // Log response details
                _logger.LogInformation("Response for {Path} is {StatusCode}", ctx.Request.Path,
                    ctx.Response.StatusCode);

                if (ctx.User.Identity?.IsAuthenticated == true)
                {
                    var userId = ctx.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    _logger.LogInformation("User id: {UserId}", userId);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while processing {Method} request to {Path}", ctx.Request.Method, ctx.Request.Path);
            throw; // Re-throw the error after logging it
        }
    }
}