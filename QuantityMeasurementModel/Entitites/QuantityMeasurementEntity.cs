using QuantityMeasurementModel.DTOs;

namespace QuantityMeasurementModel
{
    public class QuantityMeasurementEntity
    {
        public Guid Id { get; }
        public DateTime Timestamp { get; }
        public string Operation { get; }
        public QuantityDTO? Operand1 { get; }
        public QuantityDTO? Operand2 { get; }
        public QuantityDTO? Result { get; }
        public bool? BoolResult { get; }
        public double? ScalarResult { get; }
        public bool HasError { get; }
        public string ErrorMessage { get; }

        // conversion
        public QuantityMeasurementEntity(string operation, QuantityDTO operand1, QuantityDTO result)
        {
            Id = Guid.NewGuid(); Timestamp = DateTime.UtcNow;
            Operation = operation; Operand1 = operand1; Result = result;
            ErrorMessage = string.Empty;
        }

        // binary arithmetic
        public QuantityMeasurementEntity(string operation, QuantityDTO operand1, QuantityDTO operand2, QuantityDTO result)
        {
            Id = Guid.NewGuid(); Timestamp = DateTime.UtcNow;
            Operation = operation; Operand1 = operand1; Operand2 = operand2; Result = result;
            ErrorMessage = string.Empty;
        }

        // comparison
        public QuantityMeasurementEntity(string operation, QuantityDTO operand1, QuantityDTO operand2, bool boolResult)
        {
            Id = Guid.NewGuid(); Timestamp = DateTime.UtcNow;
            Operation = operation; Operand1 = operand1; Operand2 = operand2; BoolResult = boolResult;
            ErrorMessage = string.Empty;
        }

        // division
        public QuantityMeasurementEntity(string operation, QuantityDTO operand1, QuantityDTO operand2, double scalarResult)
        {
            Id = Guid.NewGuid(); Timestamp = DateTime.UtcNow;
            Operation = operation; Operand1 = operand1; Operand2 = operand2; ScalarResult = scalarResult;
            ErrorMessage = string.Empty;
        }

        // error
        public QuantityMeasurementEntity(string operation, string errorMessage)
        {
            Id = Guid.NewGuid(); Timestamp = DateTime.UtcNow;
            Operation = operation; HasError = true; ErrorMessage = errorMessage;
        }

        public override string ToString()
        {
            if (HasError) return $"[{Timestamp:u}] {Operation} ERROR: {ErrorMessage}";
            if (BoolResult != null) return $"[{Timestamp:u}] {Operation}: {Operand1} == {Operand2} → {BoolResult}";
            if (ScalarResult != null) return $"[{Timestamp:u}] {Operation}: {Operand1} ÷ {Operand2} = {ScalarResult:F4}";
            if (Operand2 != null) return $"[{Timestamp:u}] {Operation}: {Operand1} ⊕ {Operand2} = {Result}";
            return $"[{Timestamp:u}] {Operation}: {Operand1} → {Result}";
        }
    }
}