using System.Diagnostics;

namespace GPMS.Middlewares
{
    public class LoggingMiddleware
    {
       
            private readonly RequestDelegate _next;
            private readonly ILogger<LoggingMiddleware> _logger;

            public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
            {
                _next = next;
                _logger = logger;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                var stopwatch = Stopwatch.StartNew();

                _logger.LogInformation("Request: {method} {url}",
                    context.Request.Method, context.Request.Path);

                await _next(context); 

                stopwatch.Stop();
                _logger.LogInformation("Response: {statusCode} in {time}ms",
                    context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
            }
        
    }
}
