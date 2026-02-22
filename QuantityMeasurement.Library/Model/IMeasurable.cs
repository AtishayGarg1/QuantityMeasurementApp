namespace QuantityMeasurement.Library.Model
{
    public interface IMeasurable
    {
        double ConvertToBaseUnit(double value);
        double ConvertFromBaseUnit(double baseValue);
        string GetUnitName();
    }
}