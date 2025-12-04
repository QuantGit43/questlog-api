using System.Net;
using System.Text.Json;
using QuestLog.Application.Exeptions;

namespace QuestLog.Api.Middleware
{

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred while processing request.");

                var (statusCode, errorMessage) = MapExceptionToResponse(ex);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                var payload = new { error = errorMessage };
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(payload, options);
                await context.Response.WriteAsync(json);
            }
        } 
        private static (int statusCode, string message) MapExceptionToResponse(Exception ex) {
            var status = StatusCodes.Status500InternalServerError;
            var message = "An unexpected internal server error occurred.";

            switch (ex)
            {
                case ValidationException vex:
                    status = StatusCodes.Status400BadRequest;
                    message = vex.Message;
                    break;

                case ArgumentException aex:
                    status = StatusCodes.Status400BadRequest;
                    message = aex.Message;
                    break;
                
                case FormatException fex:
                    status = StatusCodes.Status400BadRequest;
                    message = "Invalid data format.";
                    break;
                
                case UnauthorizedAccessException:
                    status = StatusCodes.Status401Unauthorized;
                    message = "You are not authorized to access this resource.";
                    break;
                
                case InvalidOperationException ioex when ioex.Message.Contains("Access denied"):
                    status = StatusCodes.Status403Forbidden;
                    message = "You do not have permission to perform this action.";
                    break;
                
                case KeyNotFoundException kex:
                    status = StatusCodes.Status404NotFound;
                    message = kex.Message;
                    break;
                
                case InvalidOperationException ioex when ioex.Message.Contains("already exists"):
                    status = StatusCodes.Status409Conflict;
                    message = ioex.Message;
                    break;

                case Microsoft.EntityFrameworkCore.DbUpdateException:
                    status = StatusCodes.Status500InternalServerError;
                    message = "A database error occurred while saving changes.";
                    break;
                
                case NullReferenceException:
                    status = StatusCodes.Status500InternalServerError;
                    message = "A server error occurred (Null Reference). Please contact support.";
                    break;

                case TimeoutException:
                    status = StatusCodes.Status503ServiceUnavailable;
                    message = "The service is currently unavailable. Please try again later.";
                    break;
            }

            return (status, message);
        }
    }
}
