using System;

namespace QuantityMeasurementApp.Models
{
    // Generic Quantity class that works with any unit type
    public class Quantity<U> where U : Enum
    {
        public double Value { get; }
        public U Unit { get; }

        private const double EPSILON = 0.0001;

        public Quantity(double value, U unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException("Invalid quantity value");
            }
            Value = value;
            Unit = unit;
        }

        // Converts the current quantity to its category's base unit
        private double ConvertToBase()
        {
            if (Unit is LengthUnit length)
                return length.ConvertToBaseUnit(Value);

            if (Unit is WeightUnit weight)
                return weight.ConvertToBaseUnit(Value);

            if (Unit is VolumeUnit volume)
                return volume.ConvertToBaseUnit(Value);

            throw new InvalidOperationException("Unsupported unit type");
        }

        // Converts this quantity to a target unit

        public double ConvertTo(U targetUnit)
        {
            double baseValue = ConvertToBase();

            if (targetUnit is LengthUnit length)
                return length.ConvertFromBaseUnit(baseValue);

            if (targetUnit is WeightUnit weight)
                return weight.ConvertFromBaseUnit(baseValue);

            if (targetUnit is VolumeUnit volume)
                return volume.ConvertFromBaseUnit(baseValue);

            throw new InvalidOperationException("Unsupported unit type");
        }

        // Adds two quantities using the unit of the first operand
        public Quantity<U> Add(Quantity<U> other)
        {
            return Add(other, Unit);
        }

        // Adds two quantities and returns result in a specified target unit
        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            double base1 = ConvertToBase();
            double base2 = other.ConvertToBase();

            double sumBase = base1 + base2;
            double result;

            if (targetUnit is LengthUnit length)
                result = length.ConvertFromBaseUnit(sumBase);

            else if (targetUnit is WeightUnit weight)
                result = weight.ConvertFromBaseUnit(sumBase);

            else if (targetUnit is VolumeUnit volume)
                result = volume.ConvertFromBaseUnit(sumBase);

            else
                throw new InvalidOperationException("Unsupported unit type");

            return new Quantity<U>(result, targetUnit);
        }
        // Equality comparison between two quantities
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var other = (Quantity<U>)obj;

            double base1 = ConvertToBase();
            double base2 = other.ConvertToBase();
            return Math.Abs(base1 - base2) < EPSILON;
        }

        // Hash code based on normalized base value
        public override int GetHashCode()
        {
            return ConvertToBase().GetHashCode();
        }

        public override string ToString()
        {
            return $"{Value} {Unit}";
        }
    }
}