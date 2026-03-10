using System.Net;
using System.Text.Json;

namespace JobApplicationTracker.Api.Errors
{
    internal sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception caught by middleware.");

                var (statusCode, errorInfo) = MapExceptionToError(ex);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;

                var payload = new
                {
                    ErrorCode = errorInfo.ErrorCode,
                    ErrorMessage = errorInfo.ErrorMessage
                };

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(payload, options);

                await context.Response.WriteAsync(json);
            }
        }

        private static (HttpStatusCode statusCode, ErrorInfo errorInfo) MapExceptionToError(Exception ex)
        {
            if (ex is ApiException apiEx)
            {
                // If the centralized ErrorCache contains a NotFound code, map to 404.
                if (apiEx.ErrorCode == ErrorCache.NotFound.ErrorCode)
                {
                    return (HttpStatusCode.NotFound, ErrorCache.NotFound);
                }

                // Default for ApiException is server error (500) — still use centralized info.
                // If you add more ErrorInfo entries you can add mappings here.
                return (HttpStatusCode.InternalServerError, new ErrorInfo(apiEx.ErrorCode, apiEx.ErrorMessage));
            }

            // Non-ApiException -> treat as unhandled server error, use centralized UnhandledError.
            return (HttpStatusCode.InternalServerError, ErrorCache.UnhandledError);
        }
    }

    internal static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
            => builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}