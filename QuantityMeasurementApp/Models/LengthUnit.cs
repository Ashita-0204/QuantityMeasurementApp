using QuantityMeasurementApp.Interfaces;

namespace QuantityMeasurementApp.Models
{
    public enum LengthUnit
    {
        Feet,
        Inches,
        Yards,
        Centimeters
    }

    public static class LengthUnitExtensions
    {
        public static double GetConversionFactor(this LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.Feet => 12.0,
                LengthUnit.Inches => 1.0,
                LengthUnit.Yards => 36.0,
                LengthUnit.Centimeters => 0.393701,
                _ => throw new ArgumentException("Invalid Unit")
            };
        }

        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }

        public static string GetUnitName(this LengthUnit unit)
        {
            return unit.ToString();
        }
    }
}