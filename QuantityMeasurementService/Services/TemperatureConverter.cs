using System;
using System.Collections.Generic;
using QuantityMeasurementModel;
namespace QuantityMeasurementService

{
    public class TemperatureConverter : IMeasurable<TemperatureUnit>
    {
        public static readonly TemperatureConverter Instance = new();
        private readonly Dictionary<TemperatureUnit, (double Multiplier, double Offset)> _toCelsius = new()
        {
            { TemperatureUnit.CELSIUS,    (1.0, 0.0) },
            { TemperatureUnit.KELVIN,     (1.0, -273.15) },
            { TemperatureUnit.FAHRENHEIT, (5.0 / 9.0, -160.0 / 9.0) }
        };
        public double ToBaseUnit(TemperatureUnit unit, double value)
        {
            var (multiplier, offset) = _toCelsius[unit];
            return (value * multiplier) + offset;
        }
        public double FromBaseUnit(TemperatureUnit unit, double baseValue)
        {
            var (multiplier, offset) = _toCelsius[unit];
            return (baseValue - offset) / multiplier;
        }
    }
}