using System;
using System.Collections.Generic;

namespace QuantityMeasurement.Library.Model
{
    public sealed class QuantityLength : IEquatable<QuantityLength>
    {
        private const double TOLERANCE = 0.00001;

        public double Value { get; }
        public LengthUnit Unit { get; }

        public QuantityLength(double value, LengthUnit unit)
        {
            if (!double.IsFinite(value))
                throw new ArgumentException("Value must be finite.");

            Value = value;
            Unit = unit;
        }

        // Convert current value to base unit (Feet)
        private double ConvertToBase()
        {
            return Unit.ToBaseUnit(Value);
        }

        // Convert to target unit (instance method)
        public QuantityLength ConvertTo(LengthUnit targetUnit)
        {
            double baseValue = ConvertToBase();
            double converted = targetUnit.FromBaseUnit(baseValue);
            return new QuantityLength(converted, targetUnit);
        }

        // convert from source unit to target unit
        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            var quantity = new QuantityLength(value, source);
            return quantity.ConvertTo(target).Value;
        }

        // Adds another length and returns result in current object's unit
        public QuantityLength Add(QuantityLength other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            double sumBase =
                ConvertToBase() + other.ConvertToBase();

            double result =
                Unit.FromBaseUnit(sumBase);

            return new QuantityLength(result, Unit);
        }

        // Adds 2 length and gives result in target unit
        public static QuantityLength Add(
            QuantityLength first,
            QuantityLength second,
            LengthUnit targetUnit)
        {
            if (first is null)
                throw new ArgumentNullException(nameof(first));

            if (second is null)
                throw new ArgumentNullException(nameof(second));

            double sumBase =
                first.ConvertToBase() + second.ConvertToBase();

            double result =
                targetUnit.FromBaseUnit(sumBase);

            return new QuantityLength(result, targetUnit);
        }

        // Equality comparison (cross-unit safe)
        public bool Equals(QuantityLength? other)
        {
            if (other is null)
                return false;

            return Math.Abs(
                ConvertToBase() - other.ConvertToBase()
            ) < TOLERANCE;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as QuantityLength);
        }

        public override int GetHashCode()
        {
            return ConvertToBase().GetHashCode();
        }

        public override string ToString()
        {
            return $"{Value:F2} {Unit}";
        }
    }
}