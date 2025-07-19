using System.Text.Json;
using UniBill.DTOs;

namespace UniBill.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");

                var errors = new List<string>
                {
                    ex.Message
                };
                if (ex.InnerException != null)
                {
                    errors.Add($"Inner Exeption: {ex.InnerException.Message}");
                }

                var result = CustomResult<string>.Fail("An error occurred while processing your request.", errors);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var jsonResponse = JsonSerializer.Serialize(result, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
