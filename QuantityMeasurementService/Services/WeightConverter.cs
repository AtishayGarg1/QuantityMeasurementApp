using System;
using System.Collections.Generic;

namespace QuantityMeasurementService.Core
{
    public class WeightConverter : IMeasurable
    {
        public static readonly WeightConverter Instance = new WeightConverter();

        public string MeasurementCategory => "Weight";

        private readonly Dictionary<string, double> _toKiloGrams = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
        {
            { "KILOGRAM", 1.0 },
            { "GRAM", 0.001 },
            { "POUND", 0.453592 }
        };

        public double ToBaseUnit(string unit, double value)
        {
            if (!_toKiloGrams.ContainsKey(unit)) throw new ArgumentException("Invalid unit provided.");
            return value * _toKiloGrams[unit];
        }

        public double FromBaseUnit(string unit, double baseValue)
        {
            if (!_toKiloGrams.ContainsKey(unit)) throw new ArgumentException("Invalid target unit.");
            return baseValue / _toKiloGrams[unit];
        }
    }
}