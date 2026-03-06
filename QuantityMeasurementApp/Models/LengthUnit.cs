using System;

namespace QuantityMeasurementApp.Models
{
    public enum LengthUnit
    {
        Feet,
        Inches,
        Yards,
        Centimeters
    }

    public static class LengthUnitHelper
    {
        private static double GetConversionFactor(LengthUnit unit)
        {
            switch (unit)
            {
                case LengthUnit.Feet:
                    return 12.0;

                case LengthUnit.Inches:
                    return 1.0;

                case LengthUnit.Yards:
                    return 36.0;

                case LengthUnit.Centimeters:
                    return 0.393701;

                default:
                    throw new ArgumentException("Invalid Unit");
            }
        }

        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            return Math.Round(value * GetConversionFactor(unit), 6);
        }

        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
        {
            return Math.Round(baseValue / GetConversionFactor(unit), 6);
        }
    }
}