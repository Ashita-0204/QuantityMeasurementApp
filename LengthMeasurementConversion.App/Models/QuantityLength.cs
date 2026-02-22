using LengthMeasurementConversion.App.Exceptions;
using LengthMeasurementConversion.App.Models;
namespace LengthMeasurementConversion.App.Models
{
    //Generic Length Quantity (DRY Principle)->UC3 Implementation
    //For UC4 Implementation
    public class QuantityLength
    {
        private readonly double value;
        private readonly LengthUnit unit;

        public QuantityLength(double value, LengthUnit unit)
        {
            // Numeric validation
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new QuantityMeasurementException("Invalid Length Value");
            }

            this.value = value;
            this.unit = unit;
        }

        //Base Unit
        private double ConvertToFeet()
        {
            return value * unit.GetConversionFactor();
        }

        public override bool Equals(object? obj)
        {
            // Same reference
            if (this == obj)
            {
                return true;
            }

            // Null or Different type
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            QuantityLength other = (QuantityLength)obj;

            // Compare after conversion
            return this.ConvertToFeet().CompareTo(other.ConvertToFeet()) == 0;
        }

        public override int GetHashCode()
        {
            return ConvertToFeet().GetHashCode();
        }

        // Static Conversion API--UC5

        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new QuantityMeasurementException("Invalid conversion value");
            }

            // Convertto base unit
            double baseFeet = value * source.GetFactor();

            // Convert FEET -> Target
            double result = baseFeet / target.GetFactor();

            // rounding precision
            return Math.Round(result, 6);
        }


        // Instance Conversion

        public QuantityLength ConvertTo(LengthUnit target)
        {
            double convertedValue = Convert(this.value, this.unit, target);
            return new QuantityLength(convertedValue, target);
        }
        public override string ToString()
        {
            return $"{value} {unit}";
        }

    }
}