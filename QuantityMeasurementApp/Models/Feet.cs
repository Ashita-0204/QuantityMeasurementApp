using QuantityMeasurementApp.Exceptions;
namespace QuantityMeasurementApp.Models
{
    public class Feet
    {
        private readonly double value;

        public Feet(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new QuantityMeasurementException("Invalid Feet Measurement");
            }
            this.value = value;
        }

        public override bool Equals(object? obj)
        {
            // Same reference
            if (this == obj)
            {
                return true;
            }

            // Null or different type
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Feet other = (Feet)obj;

            // Correct Floating Comparison
            return this.value.CompareTo(other.value) == 0;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}