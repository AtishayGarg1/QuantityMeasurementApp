using System.Collections.Generic;
using QuantityMeasurementModel; 
namespace QuantityMeasurementService
{
    public class LengthConverter : IMeasurable<LengthUnit>
    {
        public static readonly LengthConverter Instance = new();

        private readonly Dictionary<LengthUnit, double> _toInches = new()
        {
            { LengthUnit.INCH, 1.0 },
            { LengthUnit.FEET, 12.0 },
            { LengthUnit.YARD, 36.0 },
            { LengthUnit.CENTIMETRE, 0.39370078740157477 }
        };

        public double ToBaseUnit(LengthUnit unit, double value) => value * _toInches[unit];
        public double FromBaseUnit(LengthUnit unit, double baseValue) => baseValue / _toInches[unit];
    }
}