using QuantityMeasurementModel.Entities;
using System.Collections.Generic;

namespace QuantityMeasurementRepository.Interfaces
{
    public interface IQuantityMeasurementRepository
    {
        void SaveMeasurement(MeasurementEntity entity);
        List<MeasurementEntity> GetAllMeasurements(string userId);
        MeasurementEntity GetMeasurementById(int id, string userId);
        bool DeleteMeasurement(int id, string userId);
        List<MeasurementEntity> GetByCategory(string category, string userId);
    }
}
