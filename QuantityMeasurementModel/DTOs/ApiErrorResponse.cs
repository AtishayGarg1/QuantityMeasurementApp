using System;

namespace QuantityMeasurementModel
{
    // API error response
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }
        public DateTime Timestamp { get; set; }

        public static ApiErrorResponse Create(int code, string message, string detail)
        {
            ApiErrorResponse error = new ApiErrorResponse();
            error.StatusCode = code;
            error.Message = message;
            error.Detail = detail;
            error.Timestamp = DateTime.UtcNow;
            return error;
        }
    }
}
