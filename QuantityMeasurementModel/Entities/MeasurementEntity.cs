using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantityMeasurementModel.Entities
{
    [Table("QuantityMeasurementHistory")]
    public class MeasurementEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(50)]
        public string MeasurementCategory { get; set; }

        [MaxLength(50)]
        public string OperationType { get; set; }

        [MaxLength(50)]
        public string MeasurementUnit1 { get; set; }

        public double MeasurementValue1 { get; set; }

        [MaxLength(50)]
        public string MeasurementUnit2 { get; set; }

        public double MeasurementValue2 { get; set; }

        [MaxLength(50)]
        public string TargetMeasurementUnit { get; set; }

        public bool IsSuccess { get; set; }

        [MaxLength(255)]
        public string ErrorMessage { get; set; }

        public bool IsComparison { get; set; }
        public bool AreEqual { get; set; }
        public double CalculatedValue { get; set; }

        [MaxLength(255)]
        public string FormattedMessage { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
