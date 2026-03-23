using QuantityMeasurementModel.Entities;
using System.Collections.Generic;

namespace QuantityMeasurementRepository.Interfaces
{
    public interface IQuantityMeasurementRepository
    {
        void SaveMeasurement(MeasurementEntity entity);
        List<MeasurementEntity> GetAllMeasurements();
        MeasurementEntity GetMeasurementById(int id);
        bool DeleteMeasurement(int id);
        List<MeasurementEntity> GetByCategory(string category);
    }
}
