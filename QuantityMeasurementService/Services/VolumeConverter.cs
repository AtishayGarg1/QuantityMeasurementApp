using System;
using System.Collections.Generic;

namespace QuantityMeasurementService
{
    public class VolumeConverter : IMeasurable
    {
        public static readonly VolumeConverter Instance = new VolumeConverter();

        public string MeasurementCategory => "Volume";

        private readonly Dictionary<string, double> _toLitres = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
        {
            { "LITRE", 1.0 },
            { "MILLILITRE", 0.001 },
            { "GALLON", 3.78541 }
        };

        public double ToBaseUnit(string unit, double value)
        {
            if (!_toLitres.ContainsKey(unit)) throw new ArgumentException("Invalid unit provided.");
            return value * _toLitres[unit];
        }

        public double FromBaseUnit(string unit, double baseValue)
        {
            if (!_toLitres.ContainsKey(unit)) throw new ArgumentException("Invalid target unit.");
            return baseValue / _toLitres[unit];
        }
    }
}