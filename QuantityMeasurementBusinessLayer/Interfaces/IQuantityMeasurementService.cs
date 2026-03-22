using QuantityMeasurementModel.DTOs;

namespace QuantityMeasurementBusinessLayer
{
    public interface IQuantityMeasurementService
    {
        QuantityResponseDTO Compare(QuantityDTO a, QuantityDTO b);
        QuantityResponseDTO Convert(QuantityDTO input, string targetUnit);
        QuantityResponseDTO Add(QuantityDTO a, QuantityDTO b, string? targetUnit = null);
        QuantityResponseDTO Subtract(QuantityDTO a, QuantityDTO b, string? targetUnit = null);
        QuantityResponseDTO Divide(QuantityDTO a, QuantityDTO b);
    }
}