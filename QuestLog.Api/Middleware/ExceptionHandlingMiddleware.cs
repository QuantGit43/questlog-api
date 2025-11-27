using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;

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

        private static (int statusCode, string message) MapExceptionToResponse(Exception ex)
        {
            var status = (int)HttpStatusCode.InternalServerError;
            var message = "An unexpected internal server error occurred.";

            if (ex is QuestLog.Api.Errors.ValidationException vex)
            {
                status = (int)HttpStatusCode.BadRequest;
                message = string.IsNullOrWhiteSpace(vex.Message) ? "Validation failed." : vex.Message;
            }


            return (status, message);
        }
    }
}
