using System;
using QuantityMeasurementModel;

namespace QuantityMeasurementService
{
    // Strongly typed internal measurement value structure
    public class Quantity<TUnit> where TUnit : Enum
    {
        // Stores the numeric measurement value
        private readonly double _value;

        // Stores the unit type of this measurement
        private readonly TUnit _unit;

        // Reference to the converter for this unit type
        private readonly IMeasurable<TUnit> _converter;

        // Constructor initializes value, unit, and converter
        public Quantity(double value, TUnit unit, IMeasurable<TUnit> converter)
        {
            _value = value;
            _unit = unit;
            _converter = converter;
        }

        // Convert the current value to base, then format into target unit
        public double ConvertTo(TUnit targetUnit)
        {
            double baseValue = _converter.ToBaseUnit(_unit, _value);
            return _converter.FromBaseUnit(targetUnit, baseValue);
        }

        // Core math execution path using Base Values
        private Quantity<TUnit> PerformArithmetic(Quantity<TUnit> other, TUnit targetUnit, ArithmeticOperation operation)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            if (typeof(TUnit) == typeof(TemperatureUnit) && operation == ArithmeticOperation.Divide)
            {
                throw new InvalidOperationException("Division is not supported for Temperature measurements.");
            }

            // Convert both values to their base unit equivalents
            double val1Base = _converter.ToBaseUnit(_unit, _value);
            double val2Base = _converter.ToBaseUnit(other._unit, other._value);

            // Trigger math operations based on the operation type using if-else
            double resultBase;
            if (operation == ArithmeticOperation.Add)
            {
                resultBase = val1Base + val2Base;
            }
            else if (operation == ArithmeticOperation.Subtract)
            {
                resultBase = val1Base - val2Base;
            }
            else if (operation == ArithmeticOperation.Divide)
            {
                resultBase = val1Base / val2Base;
            }
            else
            {
                throw new InvalidOperationException("Unknown arithmetic operation");
            }

            // Convert the result from base unit to the target unit and return
            double convertedResult = _converter.FromBaseUnit(targetUnit, resultBase);
            return new Quantity<TUnit>(convertedResult, targetUnit, _converter);
        }

        // Adds two quantities and returns the result in the target unit
        public Quantity<TUnit> Add(Quantity<TUnit> other, TUnit targetUnit)
        {
            return PerformArithmetic(other, targetUnit, ArithmeticOperation.Add);
        }

        // Subtracts the other quantity from this one and returns the result in the target unit
        public Quantity<TUnit> Subtract(Quantity<TUnit> other, TUnit targetUnit)
        {
            return PerformArithmetic(other, targetUnit, ArithmeticOperation.Subtract);
        }

        // Divides this quantity by the other and returns the result in the target unit
        public Quantity<TUnit> Division(Quantity<TUnit> other, TUnit targetUnit)
        {
            return PerformArithmetic(other, targetUnit, ArithmeticOperation.Divide);
        }

        // Check if values represent the same underlying amount within tolerance
        public override bool Equals(object obj)
        {
            // Check reference equality first
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            // Check if the other object is a Quantity of the same unit type
            Quantity<TUnit> other = obj as Quantity<TUnit>;
            if (other == null)
            {
                return false;
            }

            // Convert both values to base unit and compare within tolerance
            double firstBase = _converter.ToBaseUnit(_unit, _value);
            double secondBase = _converter.ToBaseUnit(other._unit, other._value);

            return Math.Abs(firstBase - secondBase) < 1e-6;
        }
    }
}