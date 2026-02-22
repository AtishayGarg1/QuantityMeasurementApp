namespace QuantityMeasurement.Library.Model
{
    public sealed class Feet
    {
        public double Value { get; }

        public Feet(double value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            if (obj == null || obj.GetType() != typeof(Feet))
                return false;

            Feet other = (Feet)obj;

            return Value.CompareTo(other.Value) == 0;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}