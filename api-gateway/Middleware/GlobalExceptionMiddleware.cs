using System.Net;
using System.Text.Json;

namespace ApiGateway.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try { await _next(context); }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Upstream service unavailable on {Path}", context.Request.Path);
                await Write(context, HttpStatusCode.BadGateway, "Bad Gateway", "Upstream service unavailable.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled gateway exception on {Path}", context.Request.Path);
                await Write(context, HttpStatusCode.InternalServerError, "Gateway Error", "An unexpected error occurred.");
            }
        }

        private static async Task Write(HttpContext ctx, HttpStatusCode code, string error, string message)
        {
            ctx.Response.StatusCode = (int)code;
            ctx.Response.ContentType = "application/json";
            await ctx.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                timestamp = DateTime.UtcNow,
                status    = (int)code,
                error,
                message,
                path = ctx.Request.Path.Value
            }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
        }
    }
}
