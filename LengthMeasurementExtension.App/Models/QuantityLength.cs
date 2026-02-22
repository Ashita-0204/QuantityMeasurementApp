using LengthMeasurementExtension.App.Exceptions;

namespace LengthMeasurementExtension.App.Models
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
    }
}