using System;

namespace QuantityMeasurement.Library.Model
{
    public enum TemperatureUnit
    {
        Celsius,
        Fahrenheit,
        Kelvin
    }
    public static class TemperatureUnitExtensions
    {
        public static double ConvertToBaseUnit(this TemperatureUnit unit, double value)
        {
            return unit switch
            {
                TemperatureUnit.Celsius => value,
                TemperatureUnit.Fahrenheit => (value - 32) * 5 / 9,
                TemperatureUnit.Kelvin => value - 273.15,
                _ => throw new InvalidOperationException("Unsupported temperature unit")
            };
        }
        public static double ConvertFromBaseUnit(this TemperatureUnit unit, double baseValue)
        {
            return unit switch
            {
                TemperatureUnit.Celsius => baseValue,
                TemperatureUnit.Fahrenheit => (baseValue * 9 / 5) + 32,
                TemperatureUnit.Kelvin => baseValue + 273.15,
                _ => throw new InvalidOperationException("Unsupported temperature unit")
            };
        }

        // Temperature does not support arithmetic
        public static void ValidateOperationSupport(this TemperatureUnit unit, string operation)
        {
            if (operation == "Add" || operation == "Subtract" || operation == "Divide")
            {
                throw new NotSupportedException(
                    $"Operation '{operation}' is not supported for Temperature measurements.");
            }
        }
        public static Func<bool> SupportsArithmetic => () => false;
    }
}