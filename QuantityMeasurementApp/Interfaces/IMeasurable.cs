namespace QuantityMeasurementApp.Interfaces
{
    public interface IMeasurable
    {
        string GetUnitName();

        double GetConversionFactor();

        double ConvertToBaseUnit(double value);

        double ConvertFromBaseUnit(double baseValue);

        // default: arithmetic supported
        bool SupportsArithmetic()
        {
            return true;
        }

        // validate operation
        void ValidateOperationSupport(string operation)
        {
            // default: allow all operations
        }
    }
}