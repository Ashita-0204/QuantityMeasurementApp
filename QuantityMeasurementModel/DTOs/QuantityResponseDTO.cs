using QuantityMeasurementModel.DTOs;

namespace QuantityMeasurementModel.DTOs
{
    public class QuantityResponseDTO
    {
        public bool Success { get; set; }
        public string Operation { get; set; } = string.Empty;
        public QuantityDTO? Operand1 { get; set; }
        public QuantityDTO? Operand2 { get; set; }
        public QuantityDTO? Result { get; set; }
        public bool? BoolResult { get; set; }
        public double? ScalarResult { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public static QuantityResponseDTO ForEquality(string v, QuantityDTO a, QuantityDTO b, bool equal) => new()
        { Success = true, Operation = v, Operand1 = a, Operand2 = b, BoolResult = equal };

        public static QuantityResponseDTO ForConversion(QuantityDTO input, QuantityDTO output) => new()
        { Success = true, Operation = "Convert", Operand1 = input, Result = output };

        public static QuantityResponseDTO ForArithmetic(string op, QuantityDTO a, QuantityDTO b, QuantityDTO result) => new()
        { Success = true, Operation = op, Operand1 = a, Operand2 = b, Result = result };

        public static QuantityResponseDTO ForScalarMultiply(QuantityDTO a, double scalar, QuantityDTO result) => new()
        { Success = true, Operation = "Multiply", Operand1 = a, ScalarResult = scalar, Result = result };

        public static QuantityResponseDTO ForDivision(string v, QuantityDTO a, QuantityDTO b, double scalar) => new()
        { Success = true, Operation = v, Operand1 = a, Operand2 = b, ScalarResult = scalar };

        public static QuantityResponseDTO ForError(string operation, string message) => new()
        { Success = false, Operation = operation, ErrorMessage = message };

        public override string ToString() => Operation switch
        {
            _ when !Success => $"Error during {Operation}: {ErrorMessage}",
            "Compare" => $"[Compare]  {Operand1} == {Operand2}  ->  {BoolResult}",
            "Convert" => $"[Convert]  {Operand1}  ->  {Result}",
            "Multiply" => $"[Multiply] {Operand1} * {ScalarResult}  =  {Result}",
            "Divide" => $"[Divide]   {Operand1} / {Operand2}  =  {ScalarResult:F4}",
            _ => $"[{Operation}]  {Operand1} {Operation} {Operand2}  =  {Result}"
        };
    }
}