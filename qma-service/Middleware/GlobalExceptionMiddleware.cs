using System.Net;
using System.Text.Json;

namespace QmaService.Middleware
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
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Compute error on {Path}", context.Request.Path);
                await Write(context, HttpStatusCode.BadRequest, "Compute Error", ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Argument error on {Path}", context.Request.Path);
                await Write(context, HttpStatusCode.BadRequest, "Bad Request", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception on {Path}", context.Request.Path);
                await Write(context, HttpStatusCode.InternalServerError, "Internal Server Error", "An unexpected error occurred.");
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
