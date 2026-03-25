using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementModel
{
    // API request DTO
    public class MeasurementRequestDTO
    {
        [Required(ErrorMessage = "Category is required")]
        public string MeasurementCategory { get; set; }

        [Required(ErrorMessage = "Operation type is required")]
        public MeasurementAction OperationType { get; set; }

        [Required(ErrorMessage = "First unit is required")]
        public string MeasurementUnit1 { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Value1 must be non-negative")]
        public double MeasurementValue1 { get; set; }

        [Required(ErrorMessage = "Second unit is required")]
        public string MeasurementUnit2 { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Value2 must be non-negative")]
        public double MeasurementValue2 { get; set; }

        public string TargetMeasurementUnit { get; set; }
    }
}