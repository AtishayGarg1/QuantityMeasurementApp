using QuantityMeasurementModel;
namespace QuantityMeasurementService
{
    // Strongly typed internal measurement value structure 
    public class Quantity<TUnit> where TUnit : Enum
    {
        private readonly double _value;
        private readonly TUnit _unit;
        private readonly IMeasurable<TUnit> _converter;

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
            if (other == null) throw new ArgumentNullException(nameof(other));

            if (typeof(TUnit) == typeof(TemperatureUnit) && operation == ArithmeticOperation.Divide)
                throw new InvalidOperationException("Division is not supported for Temperature measurements.");

            double val1Base = _converter.ToBaseUnit(_unit, _value);
            double val2Base = _converter.ToBaseUnit(other._unit, other._value);
            
            // Trigger math operations
            double resultBase = operation switch
            {
                ArithmeticOperation.Add      => val1Base + val2Base,
                ArithmeticOperation.Subtract => val1Base - val2Base,
                ArithmeticOperation.Divide   => val1Base / val2Base,
                _                            => throw new InvalidOperationException("Unknown arithmetic operation")
            };
            
            return new Quantity<TUnit>(_converter.FromBaseUnit(targetUnit, resultBase), targetUnit, _converter);
        }

        public Quantity<TUnit> Add(Quantity<TUnit> other, TUnit targetUnit) => PerformArithmetic(other, targetUnit, ArithmeticOperation.Add);
        public Quantity<TUnit> Subtract(Quantity<TUnit> other, TUnit targetUnit) => PerformArithmetic(other, targetUnit, ArithmeticOperation.Subtract);
        public Quantity<TUnit> Division(Quantity<TUnit> other, TUnit targetUnit) => PerformArithmetic(other, targetUnit, ArithmeticOperation.Divide);

        // Check if values represent same underlying amount within tolerance
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj is not Quantity<TUnit> other) return false;
            
            double firstBase = _converter.ToBaseUnit(_unit, _value);
            double secondBase = _converter.ToBaseUnit(other._unit, other._value);

            return Math.Abs(firstBase - secondBase) < 1e-6;
        }
    }
}