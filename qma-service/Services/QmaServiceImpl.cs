using QmaService.Interfaces;
using QmaService.Models;

namespace QmaService.Services
{
    /// <summary>
    /// Stateless computation service — the Business Layer extracted as its own microservice.
    /// No database access. No repository. Pure math + unit conversion.
    /// </summary>
    public class QmaServiceImpl : IQmaService
    {
        private const double Epsilon = 0.0001;

        public QuantityResponseDTO Add(QuantityDTO a, QuantityDTO b, string? targetUnit = null)
        {
            const string op = "Add";
            try
            {
                ValidateSameCategory(a, b);
                ValidateArithmeticSupported(a, op);
                var target = targetUnit ?? a.Unit;
                var result = new QuantityDTO(
                    FromBase(ToBase(a) + ToBase(b), a.Category, target),
                    target, a.Category);
                return QuantityResponseDTO.ForArithmetic(op, a, b, result);
            }
            catch (Exception ex) { return QuantityResponseDTO.ForError(op, ex.Message); }
        }

        public QuantityResponseDTO Subtract(QuantityDTO a, QuantityDTO b, string? targetUnit = null)
        {
            const string op = "Subtract";
            try
            {
                ValidateSameCategory(a, b);
                ValidateArithmeticSupported(a, op);
                var target = targetUnit ?? a.Unit;
                var result = new QuantityDTO(
                    FromBase(ToBase(a) - ToBase(b), a.Category, target),
                    target, a.Category);
                return QuantityResponseDTO.ForArithmetic(op, a, b, result);
            }
            catch (Exception ex) { return QuantityResponseDTO.ForError(op, ex.Message); }
        }

        public QuantityResponseDTO Divide(QuantityDTO a, QuantityDTO b)
        {
            const string op = "Divide";
            try
            {
                ValidateSameCategory(a, b);
                ValidateArithmeticSupported(a, op);
                var baseB = ToBase(b);
                if (Math.Abs(baseB) < 1e-12)
                    throw new InvalidOperationException("Cannot divide by zero.");
                var scalar = ToBase(a) / baseB;
                return QuantityResponseDTO.ForDivision(op, a, b, scalar);
            }
            catch (Exception ex) { return QuantityResponseDTO.ForError(op, ex.Message); }
        }

        public QuantityResponseDTO Compare(QuantityDTO a, QuantityDTO b)
        {
            const string op = "Compare";
            try
            {
                ValidateSameCategory(a, b);
                var equal = Math.Abs(ToBase(a) - ToBase(b)) < Epsilon;
                return QuantityResponseDTO.ForEquality(op, a, b, equal);
            }
            catch (Exception ex) { return QuantityResponseDTO.ForError(op, ex.Message); }
        }

        public QuantityResponseDTO Convert(QuantityDTO input, string targetUnit)
        {
            const string op = "Convert";
            try
            {
                var converted = FromBase(ToBase(input), input.Category, targetUnit);
                var result = new QuantityDTO(converted, targetUnit, input.Category);
                return QuantityResponseDTO.ForConversion(input, result);
            }
            catch (Exception ex) { return QuantityResponseDTO.ForError(op, ex.Message); }
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private static void ValidateSameCategory(QuantityDTO a, QuantityDTO b)
        {
            if (!string.Equals(a.Category, b.Category, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException(
                    $"Cannot operate on different categories: '{a.Category}' and '{b.Category}'.");
        }

        private static void ValidateArithmeticSupported(QuantityDTO dto, string operation)
        {
            if (string.Equals(dto.Category, "Temperature", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException($"Temperature does not support {operation} operations.");
        }

        private static double ToBase(QuantityDTO dto) =>
            dto.Category.ToLowerInvariant() switch
            {
                "length"      => UnitConversions.LengthToBase(     Enum.Parse<LengthUnit>(dto.Unit, true),      dto.Value),
                "weight"      => UnitConversions.WeightToBase(     Enum.Parse<WeightUnit>(dto.Unit, true),      dto.Value),
                "volume"      => UnitConversions.VolumeToBase(     Enum.Parse<VolumeUnit>(dto.Unit, true),      dto.Value),
                "temperature" => UnitConversions.TemperatureToBase(Enum.Parse<TemperatureUnit>(dto.Unit, true), dto.Value),
                _             => throw new ArgumentException($"Unknown category: '{dto.Category}'")
            };

        private static double FromBase(double baseValue, string category, string targetUnit) =>
            category.ToLowerInvariant() switch
            {
                "length"      => UnitConversions.LengthFromBase(     Enum.Parse<LengthUnit>(targetUnit, true),      baseValue),
                "weight"      => UnitConversions.WeightFromBase(     Enum.Parse<WeightUnit>(targetUnit, true),      baseValue),
                "volume"      => UnitConversions.VolumeFromBase(     Enum.Parse<VolumeUnit>(targetUnit, true),      baseValue),
                "temperature" => UnitConversions.TemperatureFromBase(Enum.Parse<TemperatureUnit>(targetUnit, true), baseValue),
                _             => throw new ArgumentException($"Unknown category: '{category}'")
            };
    }
}
