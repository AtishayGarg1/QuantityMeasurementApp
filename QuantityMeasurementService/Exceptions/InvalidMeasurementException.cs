using System;

namespace QuantityMeasurementService
{
    // Custom exception for invalid measurements
    public class InvalidMeasurementException : Exception
    {
        public InvalidMeasurementException(string message) : base(message)
        {
        }
    }
}