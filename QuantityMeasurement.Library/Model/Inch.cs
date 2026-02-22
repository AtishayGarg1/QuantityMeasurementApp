using System;

namespace QuantityMeasurement.Library.Model
{
    public sealed class Inch : IEquatable<Inch>
    {
        public double Value { get; }

        public Inch(double value)
        {
            Value = value;
        }

        public bool Equals(Inch? other)
        {
            if (other is null)
                return false;

            return Value.CompareTo(other.Value) == 0;
        }

        public override bool Equals(object? obj)
        {
            return obj is Inch other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}