using QuantityMeasurementModel.Entities;
using System.Collections.Generic;

namespace QuantityMeasurementRepository.Interfaces
{
    // Defines the contract for saving and retrieving measurement records
    public interface IQuantityMeasurementRepository
    {
        // Persists a single measurement entity to storage
        void SaveMeasurement(MeasurementEntity entity);

        // Returns all stored measurement records as a List
        List<MeasurementEntity> GetAllMeasurements();
    }
}
