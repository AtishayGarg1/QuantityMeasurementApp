using System;

namespace QuantityMeasurement.Library.Model
{
    public interface IMeasurable
    {
        double ConvertToBaseUnit(double value);

        double ConvertFromBaseUnit(double baseValue);

        // Default arithmetic validation
        void ValidateOperationSupport(string operation)
        {
            
        }

        // Functional interface support
        Func<bool> SupportsArithmetic { get; }
    }
}