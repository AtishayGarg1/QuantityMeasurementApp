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

        public List<MeasurementEntity> GetByCategory(string category, string userId)
        {
            return _dbContext.Measurements
                .Where(m => m.MeasurementCategory == category && (m.UserId == userId || m.UserId == null))
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
        }

        public List<MeasurementEntity> GetAllMeasurements(string userId)
        {
            return _dbContext.Measurements
                .Where(m => m.UserId == userId || m.UserId == null)
                .OrderByDescending(m => m.CreatedAt)
                .ToList();
        }

        public MeasurementEntity GetMeasurementById(int id, string userId)
        {
             return _dbContext.Measurements.FirstOrDefault(m => m.Id == id && m.UserId == userId);
        }

        public bool DeleteMeasurement(int id, string userId)
        {
            var target = _dbContext.Measurements.FirstOrDefault(m => m.Id == id && m.UserId == userId);
            if (target == null) return false;

            _dbContext.Measurements.Remove(target);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
