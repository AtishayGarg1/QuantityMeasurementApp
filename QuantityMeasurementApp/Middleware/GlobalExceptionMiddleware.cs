using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QuantityMeasurementModel;

namespace QuantityMeasurementApp.Middleware
{
    // Global exception handler
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
                _logger.LogWarning(argEx, "Bad request argument");
                await WriteErrorResponse(context, HttpStatusCode.BadRequest, "Invalid argument", argEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled server error");
                await WriteErrorResponse(context, HttpStatusCode.InternalServerError, "Internal server error", ex.Message);
            }
        }

        // Write structured JSON error response
        private async Task WriteErrorResponse(HttpContext context, HttpStatusCode code, string message, string detail)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            ApiErrorResponse error = ApiErrorResponse.Create((int)code, message, detail);
            string json = JsonSerializer.Serialize(error);
            await context.Response.WriteAsync(json);
        }
    }
}
