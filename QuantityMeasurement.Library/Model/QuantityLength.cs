using System;
using System.Collections.Generic;

namespace QuantityMeasurement.Library.Model
{
    public sealed class QuantityLength : IEquatable<QuantityLength>
    {
        private const double TOLERANCE = 0.00001;

        // Conversion factors relative to base unit (Feet)
        private static readonly Dictionary<LengthUnit, double> ConversionToFeet =
            new()
            {
                { LengthUnit.Feet, 1.0 },
                { LengthUnit.Inch, 1.0 / 12.0 },
                { LengthUnit.Yard, 3.0 },
                { LengthUnit.Centimeters, 1.0 / 30.48 }
            };

        public double Value { get; }
        public LengthUnit Unit { get; }

        public QuantityLength(double value, LengthUnit unit)
        {
            if (!double.IsFinite(value))
                throw new ArgumentException("Value must be finite.");

            if (!ConversionToFeet.ContainsKey(unit))
                throw new ArgumentException("Unsupported unit.");

            Value = value;
            Unit = unit;
        }

        // Convert current value to base unit (Feet)
        private double ConvertToBase()
        {
            return Value * ConversionToFeet[Unit];
        }

        // Convert to target unit (instance method)
        public QuantityLength ConvertTo(LengthUnit targetUnit)
        {
            if (!ConversionToFeet.ContainsKey(targetUnit))
                throw new ArgumentException("Unsupported target unit.");

            double baseValue = ConvertToBase();
            double convertedValue = baseValue / ConversionToFeet[targetUnit];

            return new QuantityLength(convertedValue, targetUnit);
        }

        // static method
        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            var quantity = new QuantityLength(value, source);
            return quantity.ConvertTo(target).Value;
        }

        // Adds another length and returns result in current object's unit (instance method)
        public QuantityLength Add(QuantityLength other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            double sumInBase = ConvertToBase() + other.ConvertToBase();

            double resultValue = sumInBase / ConversionToFeet[Unit];

            return new QuantityLength(resultValue, Unit);
        }

        // Equality comparison (cross-unit safe)
        public bool Equals(QuantityLength? other)
        {
            if (other is null)
                return false;

            return Math.Abs(ConvertToBase() - other.ConvertToBase()) < TOLERANCE;
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