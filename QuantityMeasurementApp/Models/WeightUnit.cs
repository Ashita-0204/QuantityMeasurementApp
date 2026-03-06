using System;

namespace QuantityMeasurementApp.Models
{
    public enum WeightUnit
    {
        Kilogram,
        Gram,
        Pound
    }

    public static class WeightUnitHelper
    {
        private static double GetConversionFactor(WeightUnit unit)
        {
            switch (unit)
            {
                case WeightUnit.Kilogram:
                    return 1.0;

                case WeightUnit.Gram:
                    return 0.001;

                case WeightUnit.Pound:
                    return 0.453592;

                default:
                    throw new ArgumentException("Invalid Unit");
            }
        }

        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
        {
            return Math.Round(value * GetConversionFactor(unit), 6);
        }

        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
        {
            return Math.Round(baseValue / GetConversionFactor(unit), 6);
        }
    }
}