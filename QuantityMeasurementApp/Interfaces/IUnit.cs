namespace QuantityMeasurementApp.Interfaces
{
    public interface IUnit
    {
        double ConvertToBaseUnit(double value);

        double ConvertFromBaseUnit(double baseValue);
        double GetConversionFactor();
        string GetUnitName();
    }
}