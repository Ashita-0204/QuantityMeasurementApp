using System;

namespace QuantityMeasurementApp.Models
{
    public class Weight
    {
        private readonly double value;
        private readonly WeightUnit unit;

        public double Value => value;
        public WeightUnit Unit => unit;

        public Weight(double value, WeightUnit unit)
        {
            if (!double.IsFinite(value))
            {
                throw new ArgumentException("Invalid Value");
            }
            if (!Enum.IsDefined(typeof(WeightUnit), unit))
            {
                throw new ArgumentException("Invalid Unit");
            }
            this.value = value;
            this.unit = unit;
        }

        private double ConvertToBaseUnit()
        {
            return unit.ConvertToBaseUnit(value);
        }

        private double ConvertFromBase(double baseValue, WeightUnit target)
        {
            return target.ConvertFromBaseUnit(baseValue);
        }

        public bool Compare(Weight other)
        {
            if (other == null)
            {
                return false;
            }
            double thisBase = ConvertToBaseUnit();
            double otherBase = other.ConvertToBaseUnit();

            return Math.Abs(thisBase - otherBase) < 0.00001;
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
            Weight other = (Weight)obj;

            return Compare(other);
        }

        public override int GetHashCode()
        {
            return ConvertToBaseUnit().GetHashCode();
        }

        // Conversion

        public Weight ConvertTo(WeightUnit targetUnit)
        {
            if (!Enum.IsDefined(typeof(WeightUnit), targetUnit))
            {
                throw new ArgumentException("Invalid target unit");
            }
            double baseValue = ConvertToBaseUnit();
            double converted = ConvertFromBase(baseValue, targetUnit);

            return new Weight(converted, targetUnit);
        }

        // Addition (UC6 equivalent)

        public Weight Add(Weight thatWeight)
        {
            if (thatWeight == null)
            {
                throw new ArgumentException("Weight cannot be null");
            }
            double base1 = ConvertToBaseUnit();
            double base2 = thatWeight.ConvertToBaseUnit();

            double sum = base1 + base2;

            double result = ConvertFromBase(sum, this.unit);

            return new Weight(result, this.unit);
        }

        // Addition with target unit (UC7 equivalent)

        public Weight Add(Weight weight, WeightUnit targetUnit)
        {
            if (weight == null)
            {
                throw new ArgumentException("Weight cannot be null");
            }
            if (!Enum.IsDefined(typeof(WeightUnit), targetUnit))
            {
                throw new ArgumentException("Invalid target unit");
            }
            return AddAndConvert(weight, targetUnit);
        }

        private Weight AddAndConvert(Weight weight, WeightUnit targetUnit)
        {
            double base1 = ConvertToBaseUnit();
            double base2 = weight.ConvertToBaseUnit();

            double sum = base1 + base2;

            double result = ConvertFromBase(sum, targetUnit);

            return new Weight(result, targetUnit);
        }

        public override string ToString()
        {
            return $"{value:F2} {unit}";
        }
    }
}