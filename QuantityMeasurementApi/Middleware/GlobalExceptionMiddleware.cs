using System.Net;
using System.Text.Json;
using QuantityMeasurementApp.Exceptions;

namespace QuantityMeasurementApi.Middleware
{
    // It passes through this middleware If any exception occurs anywhere (controller, service, repository).
    // This middleware catches it and returns a clean JSON response
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
            try
            {
                await _next(context);
            }
            catch (QuantityMeasurementException ex)
            {
                _logger.LogWarning(ex, "Quantity error on {Path}", context.Request.Path);
                await Write(context, HttpStatusCode.BadRequest, "Quantity Measurement Error", ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized on {Path}", context.Request.Path);
                await Write(context, HttpStatusCode.Unauthorized, "Unauthorized", ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation on {Path}", context.Request.Path);
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
                status = (int)code,
                error,
                message,
                path = ctx.Request.Path.Value
            }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
        }
    }
}