using System;

namespace QuantityMeasurementApp.Models
{
    public class Quantity<U> where U : struct, Enum
    {
        public double Value { get; }
        public U Unit { get; }

        public Quantity(double value, U unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException("Invalid value");
            }
            Value = value;
            Unit = unit;
        }

        private double ConvertToBase()
        {
            if (Unit is LengthUnit l)
            {
                return l.ConvertToBaseUnit(Value);
            }
            if (Unit is WeightUnit w)
            {
                return w.ConvertToBaseUnit(Value);
            }
            throw new ArgumentException("Unsupported unit type");
        }

        public double ConvertTo(U targetUnit)
        {
            double baseValue = ConvertToBase();

            if (targetUnit is LengthUnit l)
            {
                return l.ConvertFromBaseUnit(baseValue);
            }
            if (targetUnit is WeightUnit w)
            {
                return w.ConvertFromBaseUnit(baseValue);
            }
            throw new ArgumentException("Unsupported unit type");
        }

        public Quantity<U> Add(Quantity<U> other)
        {
            return Add(other, Unit);
        }

        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            double base1 = ConvertToBase();
            double base2 = other.ConvertToBase();

            double sumBase = base1 + base2;

            double result;

            if (targetUnit is LengthUnit l)
            {
                result = l.ConvertFromBaseUnit(sumBase);
            }
            else if (targetUnit is WeightUnit w)
            {
                result = w.ConvertFromBaseUnit(sumBase);
            }
            else
            {
                throw new ArgumentException("Unsupported unit type");
            }
            return new Quantity<U>(result, targetUnit);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Quantity<U> other = (Quantity<U>)obj;

            double base1 = ConvertToBase();
            double base2 = other.ConvertToBase();

            return Math.Abs(base1 - base2) < 0.001;
        }

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