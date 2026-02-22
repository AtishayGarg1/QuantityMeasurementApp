namespace QuantityMeasurement.Library.Model
{
    public sealed class Feet
    {
        public double Value { get; }

        public Feet(double value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj is not Feet other)
                return false;

            return Value.CompareTo(other.Value) == 0;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}