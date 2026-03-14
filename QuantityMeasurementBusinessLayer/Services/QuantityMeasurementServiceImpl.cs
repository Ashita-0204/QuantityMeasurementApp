using QuantityMeasurementApp.Exceptions;
using QuantityMeasurementApp.Models;
using QuantityMeasurementModel;
using QuantityMeasurementModel.DTOs;
using QuantityMeasurementRepository;

namespace QuantityMeasurementBusinessLayer
{
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementRepository _repository;

        public QuantityMeasurementServiceImpl(IQuantityMeasurementRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public QuantityResponseDTO Compare(QuantityDTO a, QuantityDTO b)
        {
            const string op = "Compare";
            try
            {
                ValidateSameCategory(a, b);
                bool equal = Math.Abs(ToBase(a) - ToBase(b)) < 0.0001;
                _repository.Save(new QuantityMeasurementEntity(op, a, b, equal));
                return QuantityResponseDTO.ForEquality(op, a, b, equal);
            }
            catch (QuantityMeasurementException ex) { return SaveAndError(op, ex.Message); }
            catch (Exception ex) { return SaveAndError(op, ex.Message); }
        }

        public QuantityResponseDTO Convert(QuantityDTO input, string targetUnit)
        {
            const string op = "Convert";
            try
            {
                double converted = FromBase(ToBase(input), input.Category, targetUnit);
                var result = new QuantityDTO(converted, targetUnit, input.Category);
                _repository.Save(new QuantityMeasurementEntity(op, input, result));
                return QuantityResponseDTO.ForConversion(input, result);
            }
            catch (QuantityMeasurementException ex) { return SaveAndError(op, ex.Message); }
            catch (Exception ex) { return SaveAndError(op, ex.Message); }
        }

        public QuantityResponseDTO Add(QuantityDTO a, QuantityDTO b, string? targetUnit = null)
        {
            const string op = "Add";
            try
            {
                ValidateSameCategory(a, b);
                ValidateArithmeticSupported(a, op);
                string target = targetUnit ?? a.Unit;
                var result = new QuantityDTO(FromBase(ToBase(a) + ToBase(b), a.Category, target), target, a.Category);
                _repository.Save(new QuantityMeasurementEntity(op, a, b, result));
                return QuantityResponseDTO.ForArithmetic(op, a, b, result);
            }
            catch (QuantityMeasurementException ex) { return SaveAndError(op, ex.Message); }
            catch (Exception ex) { return SaveAndError(op, ex.Message); }
        }

        public QuantityResponseDTO Subtract(QuantityDTO a, QuantityDTO b, string? targetUnit = null)
        {
            const string op = "Subtract";
            try
            {
                ValidateSameCategory(a, b);
                ValidateArithmeticSupported(a, op);
                string target = targetUnit ?? a.Unit;
                var result = new QuantityDTO(FromBase(ToBase(a) - ToBase(b), a.Category, target), target, a.Category);
                _repository.Save(new QuantityMeasurementEntity(op, a, b, result));
                return QuantityResponseDTO.ForArithmetic(op, a, b, result);
            }
            catch (QuantityMeasurementException ex) { return SaveAndError(op, ex.Message); }
            catch (Exception ex) { return SaveAndError(op, ex.Message); }
        }

        public QuantityResponseDTO Divide(QuantityDTO a, QuantityDTO b)
        {
            const string op = "Divide";
            try
            {
                ValidateSameCategory(a, b);
                ValidateArithmeticSupported(a, op);
                double baseB = ToBase(b);
                if (Math.Abs(baseB) < 1e-12)
                    throw new QuantityMeasurementException("Cannot divide by zero.");
                double scalar = ToBase(a) / baseB;
                _repository.Save(new QuantityMeasurementEntity(op, a, b, scalar));
                return QuantityResponseDTO.ForDivision(op, a, b, scalar);
            }
            catch (QuantityMeasurementException ex) { return SaveAndError(op, ex.Message); }
            catch (Exception ex) { return SaveAndError(op, ex.Message); }
        }

        // ---- private helpers ----

        private QuantityResponseDTO SaveAndError(string op, string msg)
        {
            _repository.Save(new QuantityMeasurementEntity(op, msg));
            return QuantityResponseDTO.ForError(op, msg);
        }

        private static void ValidateSameCategory(QuantityDTO a, QuantityDTO b)
        {
            if (!string.Equals(a.Category, b.Category, StringComparison.OrdinalIgnoreCase))
                throw new QuantityMeasurementException(
                    $"Cannot operate on different categories: '{a.Category}' and '{b.Category}'.");
        }

        private static void ValidateArithmeticSupported(QuantityDTO dto, string operation)
        {
            if (string.Equals(dto.Category, "Temperature", StringComparison.OrdinalIgnoreCase))
                throw new QuantityMeasurementException(
                    $"Temperature does not support {operation} operations.");
        }

        private static double ToBase(QuantityDTO dto) => dto.Category.ToLowerInvariant() switch
        {
            "length" => Enum.Parse<LengthUnit>(dto.Unit, true).ConvertToBaseUnit(dto.Value),
            "weight" => Enum.Parse<WeightUnit>(dto.Unit, true).ConvertToBaseUnit(dto.Value),
            "volume" => Enum.Parse<VolumeUnit>(dto.Unit, true).ConvertToBaseUnit(dto.Value),
            "temperature" => Enum.Parse<TemperatureUnit>(dto.Unit, true).ConvertToBaseUnit(dto.Value),
            _ => throw new QuantityMeasurementException($"Unknown category: '{dto.Category}'")
        };

        private static double FromBase(double baseValue, string category, string targetUnit) =>
            category.ToLowerInvariant() switch
            {
                "length" => Enum.Parse<LengthUnit>(targetUnit, true).ConvertFromBaseUnit(baseValue),
                "weight" => Enum.Parse<WeightUnit>(targetUnit, true).ConvertFromBaseUnit(baseValue),
                "volume" => Enum.Parse<VolumeUnit>(targetUnit, true).ConvertFromBaseUnit(baseValue),
                "temperature" => Enum.Parse<TemperatureUnit>(targetUnit, true).ConvertFromBaseUnit(baseValue),
                _ => throw new QuantityMeasurementException($"Unknown category: '{category}'")
            };
    }
}