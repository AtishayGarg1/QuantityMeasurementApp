using System;
using System.Collections.Generic;

namespace QuantityMeasurement.Library.Model
{
    public sealed class Quantity<U> : IEquatable<Quantity<U>>
        where U : struct, Enum
    {
        private const double TOLERANCE = 1e-6;

        public double Value { get; }
        public U Unit { get; }

        public Quantity(double value, U unit)
        {
            if (!double.IsFinite(value))
                throw new ArgumentException("Value must be finite.");

            Value = value;
            Unit = unit;
        }


        // Convert current value to base unit
        private double ConvertToBase()
        {
            return Unit switch
            {
                LengthUnit l => l.ConvertToBaseUnit(Value),
                WeightUnit w => w.ConvertToBaseUnit(Value),
                VolumeUnit v => v.ConvertToBaseUnit(Value),
                _ => throw new InvalidOperationException("Unsupported category")
            };
        }

        // VALIDATION HELPER 

        private void ValidateArithmeticOperands(Quantity<U> other, ArithmeticOperation operation)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            if (!double.IsFinite(other.Value))
                throw new ArgumentException("Operand value must be finite.");

             // UC14: prevent arithmetic on temperature
            if (Unit is TemperatureUnit)
                throw new NotSupportedException(
                    $"Operation '{operation}' is not supported for Temperature measurements.");
        }

        // Centralized Arethmatic

        private double PerformBaseArithmetic(Quantity<U> other, ArithmeticOperation operation)
        {
            ValidateArithmeticOperands(other, operation);
            double a = ConvertToBase();
            double b = other.ConvertToBase();

            return operation switch
            {
                ArithmeticOperation.Add => a + b,
                ArithmeticOperation.Subtract => a - b,
                ArithmeticOperation.Divide => Math.Abs(b) < TOLERANCE
                    ? throw new DivideByZeroException("Cannot divide by zero quantity.")
                    : a / b,
                _ => throw new InvalidOperationException("Unsupported operation")
            };
        }

        // Convert to target unit (instance method)
        public Quantity<U> ConvertTo(U targetUnit)
        {
            double baseValue = ConvertToBase();

            double converted = targetUnit switch
            {
                LengthUnit l => l.ConvertFromBaseUnit(baseValue),
                WeightUnit w => w.ConvertFromBaseUnit(baseValue),
                VolumeUnit v => v.ConvertFromBaseUnit(baseValue),
                _ => throw new InvalidOperationException("Unsupported category")
            };

            return new Quantity<U>(converted, targetUnit);
        }

        // Adds another length and returns result in current object's unit
        public Quantity<U> Add(Quantity<U> other)
        {
            double sumBase = PerformBaseArithmetic(other, ArithmeticOperation.Add);

            double result = Unit switch
            {
                LengthUnit l => l.ConvertFromBaseUnit(sumBase),
                WeightUnit w => w.ConvertFromBaseUnit(sumBase),
                VolumeUnit v => v.ConvertFromBaseUnit(sumBase),
                _ => throw new InvalidOperationException("Unsupported category")
            };

            return new Quantity<U>(result, Unit);
        }

        // Subtract and return result in this unit
        public Quantity<U> Subtract(Quantity<U> other)
        {
            double diffBase = PerformBaseArithmetic(other, ArithmeticOperation.Subtract);

            double result = Unit switch
            {
                LengthUnit l => l.ConvertFromBaseUnit(diffBase),
                WeightUnit w => w.ConvertFromBaseUnit(diffBase),
                VolumeUnit v => v.ConvertFromBaseUnit(diffBase),
                _ => throw new InvalidOperationException("Unsupported category")
            };

            return new Quantity<U>(Math.Round(result, 2), Unit);
        }

        // Subtract and return result in specified target unit
        public Quantity<U> Subtract(Quantity<U> other, U targetUnit)
        {
            double diffBase = PerformBaseArithmetic(other, ArithmeticOperation.Subtract);

            double result = targetUnit switch
            {
                LengthUnit l => l.ConvertFromBaseUnit(diffBase),
                WeightUnit w => w.ConvertFromBaseUnit(diffBase),
                VolumeUnit v => v.ConvertFromBaseUnit(diffBase),
                _ => throw new InvalidOperationException("Unsupported category")
            };

            return new Quantity<U>(Math.Round(result, 2), targetUnit);
        }

        // Divide and return result 
        public double Divide(Quantity<U> other)
        {
            double result = PerformBaseArithmetic(other, ArithmeticOperation.Divide);
            return Math.Round(result, 2);
        }

        // Equality comparison (cross-unit safe)
        public bool Equals(Quantity<U>? other)
        {
            if (other is null)
                return false;

            return Math.Abs( ConvertToBase() - other.ConvertToBase() ) < TOLERANCE;
        }

        public override bool Equals(object? obj)
        {
            return obj is Quantity<U> other && Equals(other);
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