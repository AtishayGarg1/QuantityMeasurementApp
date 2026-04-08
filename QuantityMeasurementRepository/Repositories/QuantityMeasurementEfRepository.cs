using System;
using System.Collections.Generic;
using System.Linq;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementRepository.Interfaces;

namespace QuantityMeasurementRepository.Repositories
{
    public class QuantityMeasurementEfRepository : IQuantityMeasurementRepository
    {
        private readonly MeasurementDbContext _dbContext;

        public QuantityMeasurementEfRepository(MeasurementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SaveMeasurement(MeasurementEntity entity)
        {
            _dbContext.Measurements.Add(entity);
            _dbContext.SaveChanges();
        }

        public List<MeasurementEntity> GetByCategory(string category)
        {
            return _dbContext.Measurements
                .Where(m => m.MeasurementCategory == category)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
        }

        public List<MeasurementEntity> GetAllMeasurements()
        {
            return _dbContext.Measurements
                .OrderByDescending(m => m.CreatedAt)
                .ToList();
        }

        public MeasurementEntity GetMeasurementById(int id) => _dbContext.Measurements.Find(id);

        public bool DeleteMeasurement(int id)
        {
            var target = _dbContext.Measurements.Find(id);
            if (target == null) return false;

            _dbContext.Measurements.Remove(target);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
