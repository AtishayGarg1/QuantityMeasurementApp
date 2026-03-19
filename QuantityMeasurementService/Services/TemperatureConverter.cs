using System;
using System.Collections.Generic;
using QuantityMeasurementModel;

namespace QuantityMeasurementService
{
    // Converts temperature units to and from a base unit (Celsius)
    // Uses a multiplier and offset approach for linear conversion
    public class TemperatureConverter : IMeasurable<TemperatureUnit>
    {
        // Singleton instance for shared use across the application
        public static readonly TemperatureConverter Instance = new TemperatureConverter();

        // Dictionary mapping each temperature unit to its multiplier and offset for Celsius conversion
        private readonly Dictionary<TemperatureUnit, double[]> _toCelsius = new Dictionary<TemperatureUnit, double[]>
        {
            { TemperatureUnit.CELSIUS,    new double[] { 1.0, 0.0 } },
            { TemperatureUnit.KELVIN,     new double[] { 1.0, -273.15 } },
            { TemperatureUnit.FAHRENHEIT, new double[] { 5.0 / 9.0, -160.0 / 9.0 } }
        };

        // Converts a value from the given unit to the base unit (Celsius)
        public double ToBaseUnit(TemperatureUnit unit, double value)
        {
            double[] factors = _toCelsius[unit];
            double multiplier = factors[0];
            double offset = factors[1];
            return (value * multiplier) + offset;
        }

        // Converts a base unit value (Celsius) to the given target unit
        public double FromBaseUnit(TemperatureUnit unit, double baseValue)
        {
            double[] factors = _toCelsius[unit];
            double multiplier = factors[0];
            double offset = factors[1];
            return (baseValue - offset) / multiplier;
        }
    }
}