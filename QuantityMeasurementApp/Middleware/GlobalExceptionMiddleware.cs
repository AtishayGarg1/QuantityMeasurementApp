using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QuantityMeasurementModel;

namespace QuantityMeasurementApp.Middleware
{
    // Catch-all for measurement errors
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
            catch (ArgumentException argEx)
            {
                _logger.LogWarning(argEx, "Invalid field in request");
                await RespondWithError(context, HttpStatusCode.BadRequest, "Bad Request", argEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "System failed to process request");
                await RespondWithError(context, HttpStatusCode.InternalServerError, "Internal Server Error", "Something went wrong internally.");
            }
        }

        private async Task RespondWithError(HttpContext context, HttpStatusCode code, string title, string details)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var errorData = ApiErrorResponse.Create((int)code, title, details);
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorData));
        }
    }
}

