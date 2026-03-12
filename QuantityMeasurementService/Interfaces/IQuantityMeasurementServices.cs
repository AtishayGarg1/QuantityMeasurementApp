using QuantityMeasurementModel;

namespace QuantityMeasurementService
{
    public interface IQuantityMeasurementService
    {
        MeasurementResponseDTO ProcessMeasurement(MeasurementRequestDTO request);
    }
}