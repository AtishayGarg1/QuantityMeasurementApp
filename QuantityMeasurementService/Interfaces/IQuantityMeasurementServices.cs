using QuantityMeasurementModel;
using QuantityMeasurementModel.Entities;
using System.Collections.Generic;

namespace QuantityMeasurementService.Core
{
    public interface IQuantityMeasurementService
    {
        MeasurementResponseDTO ProcessMeasurement(MeasurementRequestDTO request);
        List<MeasurementEntity> GetMeasurementHistory();
        MeasurementEntity GetMeasurementById(int id);
        bool DeleteMeasurement(int id);
        List<MeasurementEntity> GetMeasurementsByCategory(string category);
    }
}