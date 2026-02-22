using LengthMeasurementExtension.App.Exceptions;

namespace LengthMeasurementExtension.App.Models
{
    // UC2 -> Inches Measurement Class
    public class Inches
    {
        private readonly double value;

        public Inches(double value)
        {
            // Validate numeric input
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new QuantityMeasurementException("Invalid Inches Measurement");
            }

            this.value = value;
        }

        public override bool Equals(object? obj)
        {
            // Same Reference Check
            if (this == obj)
            {
                return true;
            }

            // Null or Different Type Check
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Inches other = (Inches)obj;

            // Floating Point Comparison
            return this.value.CompareTo(other.value) == 0;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}