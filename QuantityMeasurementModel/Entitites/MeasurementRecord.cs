using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementModel.Entities
{
    /// <summary>
    /// UC17: Flat EF Core entity mapping to 'quantity_measurements_ef' table.
    /// UserId links every record to the user who saved it.
    /// </summary>
    public class MeasurementRecord
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Which user this record belongs to (matches UserEntity.Id). Nullable so
        // rows that existed before auth was added don't violate the FK constraint.
        public int? UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Operation { get; set; } = string.Empty;

        public double? Operand1Value { get; set; }
        public string? Operand1Unit { get; set; }
        public string? Operand1Category { get; set; }

        public double? Operand2Value { get; set; }
        public string? Operand2Unit { get; set; }
        public string? Operand2Category { get; set; }

        public double? ResultValue { get; set; }
        public string? ResultUnit { get; set; }
        public string? ResultCategory { get; set; }

        public bool? BoolResult { get; set; }
        public double? ScalarResult { get; set; }

        public bool HasError { get; set; }
        public string? ErrorMessage { get; set; }
    }
}