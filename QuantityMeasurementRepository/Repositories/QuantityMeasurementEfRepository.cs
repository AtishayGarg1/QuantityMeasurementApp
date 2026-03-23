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
            SetTimestampIfDefault(entity);
            _dbContext.Measurements.Add(entity);
            _dbContext.SaveChanges();
        }

        public List<MeasurementEntity> GetAllMeasurements()
        {
            return _dbContext.Measurements
                .OrderByDescending(m => m.CreatedAt)
                .ToList();
        }

        public MeasurementEntity GetMeasurementById(int id)
        {
            return _dbContext.Measurements.Find(id);
        }

        public bool DeleteMeasurement(int id)
        {
            MeasurementEntity entity = _dbContext.Measurements.Find(id);
            if (entity == null)
            {
                return false;
            }
            _dbContext.Measurements.Remove(entity);
            _dbContext.SaveChanges();
            return true;
        }

        public List<MeasurementEntity> GetByCategory(string category)
        {
            return _dbContext.Measurements
                .Where(m => m.MeasurementCategory == category)
                .OrderByDescending(m => m.CreatedAt)
                .ToList();
        }

        private void SetTimestampIfDefault(MeasurementEntity entity)
        {
            if (entity.CreatedAt == default(DateTime))
            {
                entity.CreatedAt = DateTime.Now;
            }
        }
    }
}
