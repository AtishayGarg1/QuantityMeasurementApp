namespace QuantityMeasurement.Library.Model
{
    // Supported length units with conversion factor relative to base unit (Feet).
    public enum LengthUnit
    {
        Feet,
        Inch,
        Yard,
        Centimeters
    }

    public static class LengthUnitExtensions
    {
        public static double ToBaseUnit(this LengthUnit unit, double value)
        {
            return unit switch
            {
                LengthUnit.Feet => value,
                LengthUnit.Inch => value / 12.0,
                LengthUnit.Yard => value * 3.0,
                LengthUnit.Centimeters => value / 30.48,
                _ => throw new ArgumentException("Unsupported unit.")
            };
        }

        public static double FromBaseUnit(this LengthUnit unit, double baseValue)
        {
            return unit switch
            {
                LengthUnit.Feet => baseValue,
                LengthUnit.Inch => baseValue * 12.0,
                LengthUnit.Yard => baseValue / 3.0,
                LengthUnit.Centimeters => baseValue * 30.48,
                _ => throw new ArgumentException("Unsupported unit.")
            };
        }
    }
}