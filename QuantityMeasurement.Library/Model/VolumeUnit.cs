using System;

namespace QuantityMeasurement.Library.Model
{
    public enum VolumeUnit
    {
        Litre,
        Millilitre,
        Gallon
    }

    public static class VolumeUnitExtensions
    {
        public static double ConvertToBaseUnit(this VolumeUnit unit, double value)
        {
            return unit switch
            {
                VolumeUnit.Litre => value,
                VolumeUnit.Millilitre => value * 0.001,
                VolumeUnit.Gallon => value * 3.78541,
                _ => throw new ArgumentException("Unsupported unit")
            };
        }

        public static double ConvertFromBaseUnit(this VolumeUnit unit, double baseValue)
        {
            return unit switch
            {
                VolumeUnit.Litre => baseValue,
                VolumeUnit.Millilitre => baseValue / 0.001,
                VolumeUnit.Gallon => baseValue / 3.78541,
                _ => throw new ArgumentException("Unsupported unit")
            };
        }
    }
}