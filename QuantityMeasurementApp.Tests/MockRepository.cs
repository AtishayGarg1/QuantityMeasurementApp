using System;
using System.Collections.Generic;
using System.Linq;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementRepository.Interfaces;

namespace QuantityMeasurementApp.Tests
{
    // Mock repository for unit testing
    public class MockRepository : IQuantityMeasurementRepository
    {
        private readonly List<MeasurementEntity> _store = new List<MeasurementEntity>();
        private int _nextId = 1;

        public void SaveMeasurement(MeasurementEntity entity)
        {
            entity.Id = _nextId;
            _nextId = _nextId + 1;
            _store.Add(entity);
        }

        public List<MeasurementEntity> GetAllMeasurements()
        {
            return _store;
        }

        public MeasurementEntity GetMeasurementById(int id)
        {
            return _store.FirstOrDefault(m => m.Id == id)!;
        }

        public bool DeleteMeasurement(int id)
        {
            MeasurementEntity? found = _store.FirstOrDefault(m => m.Id == id);
            if (found == null) return false;
            _store.Remove(found);
            return true;
        }

        public List<MeasurementEntity> GetByCategory(string category)
        {
            return _store.Where(m => m.MeasurementCategory == category).ToList();
        }
    }
}
