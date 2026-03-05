using System;

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

        public Length(double value, LengthUnit unit)
        {
            if (value < 0)
            {
                throw new ArgumentException("Invalid Value");
            }
            this.value = value;
            this.unit = unit;
        }
        public bool Compare(Length other)
        {
            if (other == null)
                return false;

            double thisValue = ConvertToBaseUnit();
            double otherValue = other.ConvertToBaseUnit();

            return Math.Abs(thisValue - otherValue) < 0.00001;
        }

        private static double GetConversionFactor(
            LengthUnit unit)
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
            return value * GetConversionFactor(unit);
        }
        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            if (obj == null || GetType() != obj.GetType())
                return false;

            Length other = (Length)obj;

            return Compare(other);
        }

        public override int GetHashCode()
        {
            return ConvertToBaseUnit().GetHashCode();
        }
        // UC5 STATIC API

        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            if (!double.IsFinite(value))
            {
                throw new ArgumentException("Invalid Value");
            }
            double inches = value * GetConversionFactor(source);
            return Math.Round(inches / GetConversionFactor(target), 6);
        }

        public Length ConvertTo(LengthUnit targetUnit)
        {
            double converted = Convert(this.value, this.unit, targetUnit);
            return new Length(converted, targetUnit);
        }

        public override string ToString()
        {
            return $"{value:F2} {unit}";
        }
    }
}