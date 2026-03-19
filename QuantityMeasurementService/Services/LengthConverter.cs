using System.Collections.Generic;
using QuantityMeasurementModel;

namespace QuantityMeasurementService
{
    // Converts length units to and from a base unit (inches)
    public class LengthConverter : IMeasurable<LengthUnit>
    {
        // Singleton instance for shared use across the application
        public static readonly LengthConverter Instance = new LengthConverter();

        // Dictionary mapping each length unit to its equivalent in inches
        private readonly Dictionary<LengthUnit, double> _toInches = new Dictionary<LengthUnit, double>
        {
            { LengthUnit.INCH, 1.0 },
            { LengthUnit.FEET, 12.0 },
            { LengthUnit.YARD, 36.0 },
            { LengthUnit.CENTIMETRE, 0.39370078740157477 }
        };

        // Converts a value from the given unit to the base unit (inches)
        public double ToBaseUnit(LengthUnit unit, double value)
        {
            return value * _toInches[unit];
        }

        // Converts a base unit value (inches) to the given target unit
        public double FromBaseUnit(LengthUnit unit, double baseValue)
        {
            return baseValue / _toInches[unit];
        }
    }
}