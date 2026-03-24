using System;
using System.Collections.Generic;

namespace QuantityMeasurementService.Core
{
    public class LengthConverter : IMeasurable
    {
        public static readonly LengthConverter Instance = new LengthConverter();

        public string MeasurementCategory => "Length";

        private readonly Dictionary<string, double> _toInches = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
        {
            { "INCH", 1.0 },
            { "FEET", 12.0 },
            { "YARD", 36.0 },
            { "CENTIMETRE", 0.39370078740157477 }
        };

        public double ToBaseUnit(string unit, double value)
        {
            if (!_toInches.ContainsKey(unit)) throw new ArgumentException("Invalid unit provided.");
            return value * _toInches[unit];
        }

        public double FromBaseUnit(string unit, double baseValue)
        {
            if (!_toInches.ContainsKey(unit)) throw new ArgumentException("Invalid target unit.");
            return baseValue / _toInches[unit];
        }
    }
}