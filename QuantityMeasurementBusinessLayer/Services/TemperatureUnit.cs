using System;
using QuantityMeasurementApp.Interfaces;
using QuantityMeasurementApp.Exceptions;

namespace QuantityMeasurementApp.Models
{
    public enum TemperatureUnit
    {
        Celsius,
        Fahrenheit,
        Kelvin
    }

    public static class TemperatureUnitHelper
    {
        public static bool SupportsArithmetic(this TemperatureUnit unit) => false;
        public static bool SupportsArithmetic(this LengthUnit unit) => true;
        public static bool SupportsArithmetic(this WeightUnit unit) => true;
        public static bool SupportsArithmetic(this VolumeUnit unit) => true;

        public static string GetUnitName(this TemperatureUnit unit) => unit.ToString();

        public static void ValidateOperationSupport(this TemperatureUnit unit, string operation)
        {
            throw new QuantityMeasurementException(
                $"{unit} does not support {operation} operations.");
        }

        public static double ConvertToBaseUnit(this TemperatureUnit unit, double value)
        {
            switch (unit)
            {
                case TemperatureUnit.Celsius:
                    return value;
                case TemperatureUnit.Fahrenheit:
                    return (value - 32) * 5 / 9;
                case TemperatureUnit.Kelvin:
                    return value - 273.15;
                default:
                    throw new ArgumentException("Invalid temperature unit");
            }
        }

        public static double ConvertFromBaseUnit(this TemperatureUnit unit, double baseValue)
        {
            switch (unit)
            {
                case TemperatureUnit.Celsius:
                    return baseValue;
                case TemperatureUnit.Fahrenheit:
                    return baseValue * 9 / 5 + 32;
                case TemperatureUnit.Kelvin:
                    return baseValue + 273.15;
                default:
                    throw new ArgumentException("Invalid temperature unit");
            }
        }
    }
}
