using QmaService.Models;

namespace QmaService.Interfaces
{
    public interface IQmaService
    {
        QuantityResponseDTO Add(QuantityDTO a, QuantityDTO b, string? targetUnit = null);
        QuantityResponseDTO Subtract(QuantityDTO a, QuantityDTO b, string? targetUnit = null);
        QuantityResponseDTO Divide(QuantityDTO a, QuantityDTO b);
        QuantityResponseDTO Compare(QuantityDTO a, QuantityDTO b);
        QuantityResponseDTO Convert(QuantityDTO input, string targetUnit);
    }
}
