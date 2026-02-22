using System;

namespace QuantityMeasurement.Library.Model
{
    public sealed class QuantityWeight : IEquatable<QuantityWeight>
    {
        private const double TOLERANCE = 1e-6;

        public double Value { get; }
        public WeightUnit Unit { get; }

        public QuantityWeight(double value, WeightUnit unit)
        {
            if (!double.IsFinite(value))
                throw new ArgumentException("Value must be finite.");

            Value = value;
            Unit = unit;
        }

        private double ConvertToBase()
        {
            return Unit.ToBaseUnit(Value);
        }

        public QuantityWeight ConvertTo(WeightUnit targetUnit)
        {
            double baseValue = ConvertToBase();
            double converted = targetUnit.FromBaseUnit(baseValue);
            return new QuantityWeight(converted, targetUnit);
        }

        public static double Convert(double value, WeightUnit source, WeightUnit target)
        {
            var weight = new QuantityWeight(value, source);
            return weight.ConvertTo(target).Value;
        }

        public QuantityWeight Add(QuantityWeight other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            double sumBase = ConvertToBase() + other.ConvertToBase();
            double result = Unit.FromBaseUnit(sumBase);

            return new QuantityWeight(result, Unit);
        }

        public static QuantityWeight Add(
            QuantityWeight first,
            QuantityWeight second,
            WeightUnit targetUnit)
        {
            if (first is null)
                throw new ArgumentNullException(nameof(first));

            if (second is null)
                throw new ArgumentNullException(nameof(second));

            double sumBase =
                first.ConvertToBase() + second.ConvertToBase();

            double result =
                targetUnit.FromBaseUnit(sumBase);

            return new QuantityWeight(result, targetUnit);
        }

        public bool Equals(QuantityWeight? other)
        {
            if (other is null)
                return false;

            return Math.Abs(
                ConvertToBase() - other.ConvertToBase()
            ) < TOLERANCE;
        }

        public override bool Equals(object? obj)
        {
            return obj is QuantityWeight other && Equals(other);
        }

        public override int GetHashCode()
        {
            return ConvertToBase().GetHashCode();
        }

        public override string ToString()
        {
            return $"{Value:F4} {Unit}";
        }
    }
}
