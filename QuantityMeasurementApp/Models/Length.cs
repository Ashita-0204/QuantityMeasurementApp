namespace QuantityMeasurementApp.Models
{
    public class Length
    {
        private double value;
        private LengthUnit unit;

        public enum LengthUnit
        {
            Feet,
            Inches,
            Yards,
            Centimeters
        }
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

        private double ConvertToBaseUnit()
        {
            double result = value * GetConversionFactor();
            return Math.Round(result, 5);
        }

        public bool Compare(Length other)
        {
            if (other == null)
            {
                return false;
            }

            return ConvertToBaseUnit() == other.ConvertToBaseUnit();
        }

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