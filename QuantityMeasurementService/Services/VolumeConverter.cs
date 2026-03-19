using System.Collections.Generic;
using QuantityMeasurementModel;

namespace QuantityMeasurementService
{
    // Converts volume units to and from a base unit (litres)
    public class VolumeConverter : IMeasurable<VolumeUnit>
    {
        // Singleton instance for shared use across the application
        public static readonly VolumeConverter Instance = new VolumeConverter();

        // Dictionary mapping each volume unit to its equivalent in litres
        private readonly Dictionary<VolumeUnit, double> _toLitres = new Dictionary<VolumeUnit, double>
        {
            { VolumeUnit.LITRE, 1.0 },
            { VolumeUnit.MILLILITRE, 0.001 },
            { VolumeUnit.GALLON, 3.78541 }
        };

        // Converts a value from the given unit to the base unit (litres)
        public double ToBaseUnit(VolumeUnit unit, double value)
        {
            return value * _toLitres[unit];
        }

        // Converts a base unit value (litres) to the given target unit
        public double FromBaseUnit(VolumeUnit unit, double baseValue)
        {
            return baseValue / _toLitres[unit];
        }
    }
}