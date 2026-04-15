using System;
using System.Collections.Generic;

namespace QuantityMeasurementService
{
    public class LengthConverter : IMeasurable
    {
        public string MeasurementCategory => "Length";
        public static readonly LengthConverter Instance = new LengthConverter();

        private readonly Dictionary<string, double> _toInches = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
        {
            { "INCH", 1.0 },
            { "FEET", 12.0 },
            { "YARD", 36.0 },
            { "CENTIMETRE", 0.3937 } // Standard conversion factor
        };

        public double ToBaseUnit(string unit, double value)
        {
            if (!_toInches.ContainsKey(unit)) 
                throw new ArgumentException("Invalid unit provided.");
                
            return value * _toInches[unit];
        }

        public double FromBaseUnit(string unit, double baseValue)
        {
            if (!_toInches.ContainsKey(unit)) throw new ArgumentException("Invalid target unit.");
            return baseValue / _toInches[unit];
        }
    }
}