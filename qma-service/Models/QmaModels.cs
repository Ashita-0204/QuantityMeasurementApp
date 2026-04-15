namespace QmaService.Models
{
    // ── Input DTOs ────────────────────────────────────────────────────────────

    public class QuantityRequest
    {
        public double Value { get; set; }
        public required string Unit { get; set; }
        public required string Category { get; set; }
    }

    public class TwoQuantityRequest
    {
        public required QuantityRequest Quantity1 { get; set; }
        public required QuantityRequest Quantity2 { get; set; }
        public string? TargetUnit { get; set; }
    }

    public class ConversionRequest
    {
        public double Value { get; set; }
        public required string FromUnit { get; set; }
        public required string ToUnit { get; set; }
        public required string Category { get; set; }
    }

    // ── Internal DTO ─────────────────────────────────────────────────────────

    public class QuantityDTO
    {
        public double Value { get; }
        public string Unit { get; }
        public string Category { get; }

        public QuantityDTO(double value, string unit, string category)
        {
            Value = value;
            Unit = unit;
            Category = category;
        }
    }

    // ── Response DTOs ─────────────────────────────────────────────────────────

    public class QuantityResponseDTO
    {
        public bool Success { get; set; }
        public string Operation { get; set; } = string.Empty;
        public string? ErrorMessage { get; set; }

        public QuantityData? Operand1 { get; set; }
        public QuantityData? Operand2 { get; set; }
        public QuantityData? Result { get; set; }
        public bool? BoolResult { get; set; }
        public double? ScalarResult { get; set; }

        public static QuantityResponseDTO ForArithmetic(string op, QuantityDTO a, QuantityDTO b, QuantityDTO result) =>
            new()
            {
                Success = true, Operation = op,
                Operand1 = new QuantityData(a), Operand2 = new QuantityData(b),
                Result = new QuantityData(result)
            };

        public static QuantityResponseDTO ForConversion(QuantityDTO input, QuantityDTO result) =>
            new()
            {
                Success = true, Operation = "Convert",
                Operand1 = new QuantityData(input),
                Result = new QuantityData(result)
            };

        public static QuantityResponseDTO ForEquality(string op, QuantityDTO a, QuantityDTO b, bool equal) =>
            new()
            {
                Success = true, Operation = op,
                Operand1 = new QuantityData(a), Operand2 = new QuantityData(b),
                BoolResult = equal
            };

        public static QuantityResponseDTO ForDivision(string op, QuantityDTO a, QuantityDTO b, double scalar) =>
            new()
            {
                Success = true, Operation = op,
                Operand1 = new QuantityData(a), Operand2 = new QuantityData(b),
                ScalarResult = scalar
            };

        public static QuantityResponseDTO ForError(string op, string message) =>
            new() { Success = false, Operation = op, ErrorMessage = message };
    }

    public class QuantityData
    {
        public double Value { get; set; }
        public string Unit { get; set; }
        public string Category { get; set; }

        public QuantityData(QuantityDTO dto)
        {
            Value = dto.Value;
            Unit = dto.Unit;
            Category = dto.Category;
        }
    }
}
