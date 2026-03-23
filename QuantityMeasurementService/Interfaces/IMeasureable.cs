namespace QuantityMeasurementService
{
    public interface IMeasurable
    {
        string MeasurementCategory { get; }
        double ToBaseUnit(string unit, double value);
        double FromBaseUnit(string unit, double baseValue);
    }
}