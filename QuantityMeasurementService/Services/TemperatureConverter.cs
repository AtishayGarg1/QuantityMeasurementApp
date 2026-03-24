using System;
using System.Collections.Generic;

namespace QuantityMeasurementService.Core
{
    public class TemperatureConverter : IMeasurable
    {
        public static readonly TemperatureConverter Instance = new TemperatureConverter();

        public string MeasurementCategory => "Temperature";

        private readonly Dictionary<string, double[]> _toCelsius = new Dictionary<string, double[]>(StringComparer.OrdinalIgnoreCase)
        {
            { "CELSIUS",    new double[] { 1.0, 0.0 } },
            { "KELVIN",     new double[] { 1.0, -273.15 } },
            { "FAHRENHEIT", new double[] { 5.0 / 9.0, -160.0 / 9.0 } }
        };

        public double ToBaseUnit(string unit, double value)
        {
            if (!_toCelsius.ContainsKey(unit)) throw new ArgumentException("Invalid unit provided.");
            double[] factors = _toCelsius[unit];
            return (value * factors[0]) + factors[1];
        }

        public double FromBaseUnit(string unit, double baseValue)
        {
            if (!_toCelsius.ContainsKey(unit)) throw new ArgumentException("Invalid target unit.");
            double[] factors = _toCelsius[unit];
            return (baseValue - factors[1]) / factors[0];
        }
    }
}