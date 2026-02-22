namespace QuantityMeasurement.Library.Model
{
    public enum WeightUnit
    {
        Kilogram,
        Gram,
        Pound
    }

    public static class WeightUnitExtensions
    {
        public static double ToBaseUnit(this WeightUnit unit, double value)
        {
            return unit switch
            {
                WeightUnit.Kilogram => value,
                WeightUnit.Gram => value * 0.001,
                WeightUnit.Pound => value * 0.453592,
                _ => throw new ArgumentException("Unsupported unit.")
            };
        }

        public static double FromBaseUnit(this WeightUnit unit, double baseValue)
        {
            return unit switch
            {
                WeightUnit.Kilogram => baseValue,
                WeightUnit.Gram => baseValue / 0.001,
                WeightUnit.Pound => baseValue / 0.453592,
                _ => throw new ArgumentException("Unsupported unit.")
            };
        }
    }
}