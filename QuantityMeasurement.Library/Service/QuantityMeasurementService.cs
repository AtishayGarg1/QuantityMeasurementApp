using System;
using QuantityMeasurement.Library.Model;

namespace QuantityMeasurement.Library.Service
{
    public class QuantityMeasurementService
    {
        public bool Compare<U>(Quantity<U> first, Quantity<U> second) where U : struct, Enum
        {
            if (first == null || second == null)
                throw new ArgumentNullException("Quantity cannot be null");

            return first.Equals(second);
        }
    }
}