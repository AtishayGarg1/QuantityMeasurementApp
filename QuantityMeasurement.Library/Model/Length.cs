using System;

namespace QuantityMeasurement.Library.Model
{
    public sealed class Length : IEquatable<Length>
    {
        public double Value { get; }
        public Unit Unit { get; }

        public Length(double value, Unit unit)
        {
            Value = value;
            Unit = unit;
        }

        private double ConvertToBaseUnit()
        {
            // Base Unit = Inch

            return Unit switch
            {
                Unit.Feet => Value * 12,                 // 1 ft = 12 in
                Unit.Inch => Value,                     // base
                Unit.Yard => Value * 36,                // 1 yard = 36 in
                Unit.Centimeter => Value * 0.393701,    // 1 cm = 0.393701 in
                _ => throw new InvalidOperationException("Unsupported unit")
            };
        }

       public bool Equals(Length? other)
        {
            if (other is null)
                return false;

            double first = ConvertToBaseUnit();
            double second = other.ConvertToBaseUnit();

            // floating point comaparison tolerance
            return Math.Abs(first - second) < 0.00001;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Length);
        }

        public override int GetHashCode()
        {
            return ConvertToBaseUnit().GetHashCode();
        }
    }
}