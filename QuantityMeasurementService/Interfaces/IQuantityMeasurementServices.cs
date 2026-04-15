using QuantityMeasurementModel;
using QuantityMeasurementModel.Entities;
using System.Collections.Generic;

namespace QuantityMeasurementService
{
    public interface IQuantityMeasurementService
    {
        MeasurementResponseDTO ProcessMeasurement(MeasurementRequestDTO request, string userId);
        List<MeasurementEntity> GetMeasurementHistory(string userId);
        MeasurementEntity GetMeasurementById(int id, string userId);
        bool DeleteMeasurement(int id, string userId);
        List<MeasurementEntity> GetMeasurementsByCategory(string category, string userId);
    }
}