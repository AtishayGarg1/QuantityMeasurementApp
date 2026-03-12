namespace QuantityMeasurementService
{
    public interface IMeasurable<TUnit> where TUnit : Enum
    {
        double ToBaseUnit(TUnit unit, double value);
        double FromBaseUnit(TUnit unit, double baseValue);
    }
}