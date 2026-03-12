using System;

namespace QuantityMeasurementApp.Models
{
    public class Length
    {
        private double value;
        private LengthUnit unit;

        public double Value => value;
        public LengthUnit Unit => unit;

        public Length(double value, LengthUnit unit)
        {
            if (!double.IsFinite(value))

            {
                throw new ArgumentException("Invalid Value");
            }

            if (!Enum.IsDefined(typeof(LengthUnit), unit))
            {
                throw new ArgumentException("Invalid Unit");
            }
            if (value < 0)
                throw new ArgumentException("Invalid Value");

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

        private double ConvertToBaseUnit()
        {
            return unit.ConvertToBaseUnit(value);
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

        // UC5 Conversion

        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            if (!double.IsFinite(value))
                throw new ArgumentException("Invalid Value");

            double baseValue = source.ConvertToBaseUnit(value);

            return target.ConvertFromBaseUnit(baseValue);
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

        // UC6 Addition

        private double ConvertFromBaseToTargetUnit(double baseValue, LengthUnit targetUnit)
        {
            return targetUnit.ConvertFromBaseUnit(baseValue);
        }

        public Length Add(Length thatLength)
        {
            if (thatLength == null)
                throw new ArgumentException("Length to add cannot be null");

            double thisInches = this.ConvertToBaseUnit();
            double thatInches = thatLength.ConvertToBaseUnit();

            double sumInches = thisInches + thatInches;

            double resultValue = ConvertFromBaseToTargetUnit(sumInches, this.unit);

            return new Length(resultValue, this.unit);
        }

        // UC7 Addition with Target Unit

        public Length Add(Length thatLength, LengthUnit targetUnit)
        {
            if (thatLength == null)
                throw new ArgumentException("Length to add cannot be null");

            if (!Enum.IsDefined(typeof(LengthUnit), targetUnit))
                throw new ArgumentException("Invalid target unit");

            return AddAndConvert(thatLength, targetUnit);
        }

        private Length AddAndConvert(Length thatLength, LengthUnit targetUnit)
        {
            double thisInches = this.ConvertToBaseUnit();
            double thatInches = thatLength.ConvertToBaseUnit();

            double sumInches = thisInches + thatInches;

            double resultValue = ConvertFromBaseToTargetUnit(sumInches, targetUnit);

            return new Length(resultValue, targetUnit);
        }
    }
}