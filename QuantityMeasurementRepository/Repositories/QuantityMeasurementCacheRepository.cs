using System;
using System.Collections.Generic;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementRepository.Interfaces;

namespace QuantityMeasurementRepository.Repositories
{
    // In-memory cache repository that stores measurement records in a List
    // Suitable for testing and lightweight usage without database dependency
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        // Internal list to hold all cached measurement records
        private readonly List<MeasurementEntity> _cache;

        // Auto-incrementing ID counter for assigning unique IDs
        private int _nextId;

        // Constructor initializes the cache list and ID counter
        public QuantityMeasurementCacheRepository()
        {
            _cache = new List<MeasurementEntity>();
            _nextId = 1;
        }

        // Saves a measurement entity to the in-memory cache
        // Assigns an auto-incremented ID and timestamp if not set
        public void SaveMeasurement(MeasurementEntity entity)
        {
            // Assign auto-incremented ID to the entity
            entity.Id = _nextId;
            _nextId = _nextId + 1;

            // Set CreatedAt timestamp if it was not already set
            if (entity.CreatedAt == default(DateTime))
            {
                entity.CreatedAt = DateTime.Now;
            }

            // Add the entity to the in-memory cache list
            _cache.Add(entity);

            // Print confirmation message
            Console.WriteLine("Measurement saved to cache with ID: " + entity.Id);
        }

        // Returns all cached measurement entities as a List
        public List<MeasurementEntity> GetAllMeasurements()
        {
            // Return the full list of cached measurements
            return _cache;
        }
    }
}
