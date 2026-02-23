namespace QuantityMeasurementApp.Models
{
    public class Length
    {
        private double value;
        private LengthUnit unit;

        // Constructor
        public Length(double value, LengthUnit unit)
        {
            if (value < 0)
            {
                throw new ArgumentException("Invalid Value");
            }
            this.value = value;
            this.unit = unit;
        }

        // Conversion factor inside Length
        private double GetConversionFactor()
        {
            if (unit == LengthUnit.Feet)
            {
                return 12.0;
            }// 1 foot=12 inches

            return 1.0; //Inches base unit
        }

        // Convert to base unit(Inches)
        private double ConvertToBaseUnit()
        {
            return value * GetConversionFactor();
        }

        // Compare two Length objects
        public bool Compare(Length other)
        {
            if (other == null)
            {
                return false;
            }
            return this.ConvertToBaseUnit() == other.ConvertToBaseUnit();
        }

        // Equals Override
        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Length other = (Length)obj;

            return Compare(other);
        }

        public override int GetHashCode()
        {
            return ConvertToBaseUnit().GetHashCode();
        }
    }
}