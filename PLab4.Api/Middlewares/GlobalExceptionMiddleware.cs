using System.Text.Json;
using RestaurantSystem.Domain.Exceptions;

namespace RestaurantSystem.Api.Middlewares;

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
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Unhandled exception occurred while processing {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                KeyNotFoundException => StatusCodes.Status404NotFound,
                ConflictException => StatusCodes.Status409Conflict,
                InvalidOperationException => StatusCodes.Status400BadRequest,
                DatabaseConnectionException => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };

            var response = new
            {
                message = context.Response.StatusCode is StatusCodes.Status400BadRequest
                    or StatusCodes.Status404NotFound
                    or StatusCodes.Status409Conflict
                    ? exception.Message
                    : "An unexpected internal server error occurred.",
                errorType = exception.GetType().Name,
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
