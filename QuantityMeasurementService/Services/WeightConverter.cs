using System.Collections.Generic;
using QuantityMeasurementModel;

namespace QuantityMeasurementService
{
    // Converts weight units to and from a base unit (kilograms)
    public class WeightConverter : IMeasurable<WeightUnit>
    {
        // Singleton instance for shared use across the application
        public static readonly WeightConverter Instance = new WeightConverter();

        // Dictionary mapping each weight unit to its equivalent in kilograms
        private readonly Dictionary<WeightUnit, double> _toKiloGrams = new Dictionary<WeightUnit, double>
        {
            { WeightUnit.KILOGRAM, 1.0 },
            { WeightUnit.GRAM, 0.001 },
            { WeightUnit.POUND, 0.453592 }
        };

        // Converts a value from the given unit to the base unit (kilograms)
        public double ToBaseUnit(WeightUnit unit, double value)
        {
            return value * _toKiloGrams[unit];
        }

        // Converts a base unit value (kilograms) to the given target unit
        public double FromBaseUnit(WeightUnit unit, double baseValue)
        {
            return baseValue / _toKiloGrams[unit];
        }
    }
}