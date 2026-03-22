using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementModel.DTOs
{
    /// <summary>
    /// UC17: Request body for all quantity measurement API endpoints.
    /// For Convert: Operand1 = source, Operand2.Unit = target unit.
    /// For binary ops: supply both Operand1 and Operand2 fully.
    /// </summary>
    public class QuantityInputDTO
    {
        [Required]
        public QuantityDTO Operand1 { get; set; } = new();

        public QuantityDTO? Operand2 { get; set; }

        /// <summary>Optional target unit for Add/Subtract result.</summary>
        public string? TargetUnit { get; set; }
    }
}