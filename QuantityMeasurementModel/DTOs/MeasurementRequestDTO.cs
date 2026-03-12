namespace QuantityMeasurementModel
{
    public class MeasurementRequestDTO
    {
        public string MeasurementCategory{get; set; }
        public MeasurementAction OperationType { get; set; }
        public string MeasurementUnit1 { get; set; }
        public double MeasurementValue1 { get; set; }
        public string MeasurementUnit2 { get; set; }
        public double MeasurementValue2 { get; set; }
        public string TargetMeasurementUnit { get; set; }
    }
}