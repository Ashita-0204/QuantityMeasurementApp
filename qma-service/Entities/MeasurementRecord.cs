namespace QmaService.Entities
{
    public class MeasurementRecord
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int UserId { get; set; }
        public string Operation { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Operand 1
        public double? Operand1Value { get; set; }
        public string? Operand1Unit { get; set; }
        public string? Operand1Category { get; set; }

        // Operand 2
        public double? Operand2Value { get; set; }
        public string? Operand2Unit { get; set; }

        // Result
        public double? ResultValue { get; set; }
        public string? ResultUnit { get; set; }
        public string? ResultCategory { get; set; }
        public bool? BoolResult { get; set; }
        public double? ScalarResult { get; set; }
    }
}
