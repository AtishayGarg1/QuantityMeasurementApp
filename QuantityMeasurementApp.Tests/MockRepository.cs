using System;
using System.Collections.Generic;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementRepository.Interfaces;

namespace QuantityMeasurementApp.Tests
{
    // Mock repository used in unit tests to avoid actual database dependency
    // Implements the IQuantityMeasurementRepository interface with no-op operations
    public class MockRepository : IQuantityMeasurementRepository
    {
        // SaveMeasurement does nothing in the mock - prevents test side effects
        public void SaveMeasurement(MeasurementEntity entity)
        {
            // No operation needed for unit testing
        }

        // Returns an empty List of MeasurementEntity for test isolation
        public List<MeasurementEntity> GetAllMeasurements()
        {
            List<MeasurementEntity> emptyList = new List<MeasurementEntity>();
            return emptyList;
        }
    }
}
