using System;

namespace QuantityMeasurementModel.Entities
{
    public class MeasurementEntity
    {
        public int Id { get; set; }
        public string MeasurementCategory { get; set; }
        public string OperationType { get; set; }
        public string MeasurementUnit1 { get; set; }
        public double MeasurementValue1 { get; set; }
        public string MeasurementUnit2 { get; set; }
        public double MeasurementValue2 { get; set; }
        public string TargetMeasurementUnit { get; set; }
        
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsComparison { get; set; }
        public bool AreEqual { get; set; }
        public double CalculatedValue { get; set; }
        public string FormattedMessage { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}
