using System;

namespace QuantityMeasurementApp.Models
{
    public class Quantity<U> where U : Enum
    {
        // ---------------- UC13 ENUM ----------------
        private enum ArithmeticOperation
        {
            Add,
            Subtract,
            Divide
        }

        public double Value { get; }
        public U Unit { get; }

        private const double EPSILON = 0.0001;

        // ---------------- CONSTRUCTOR ----------------
        public Quantity(double value, U unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException("Invalid quantity value");
            }

            Value = value;
            Unit = unit;
        }

        // ---------------- VALIDATION ----------------
        private void ValidateArithmeticOperands(Quantity<U> other, U targetUnit, bool targetUnitRequired)
        {
            if (other == null)
            {
                throw new ArgumentException("Other quantity cannot be null");
            }

            if (!Unit.GetType().Equals(other.Unit.GetType()))
            {
                throw new ArgumentException("Incompatible unit categories");
            }

            if (double.IsNaN(Value) || double.IsInfinity(Value) ||
                double.IsNaN(other.Value) || double.IsInfinity(other.Value))
            {
                throw new ArgumentException("Invalid numeric value");
            }

            if (targetUnitRequired && targetUnit == null)
            {
                throw new ArgumentException("Target unit cannot be null");
            }
        }

        // ---------------- BASE CONVERSION ----------------
        private double ConvertToBase()
        {
            if (Unit is LengthUnit length)
            {
                return length.ConvertToBaseUnit(Value);
            }
            if (Unit is WeightUnit weight)
            {
                return weight.ConvertToBaseUnit(Value);
            }
            if (Unit is VolumeUnit volume)
            {
                return volume.ConvertToBaseUnit(Value);
            }
            if (Unit is TemperatureUnit temp)
            {
                return temp.ConvertToBaseUnit(Value);
            }
            throw new InvalidOperationException("Unsupported unit type");
        }

        // ---------------- TARGET CONVERSION ----------------
        private double ConvertBaseToTarget(double baseValue, U targetUnit)
        {
            if (targetUnit is LengthUnit length)
            {
                return length.ConvertFromBaseUnit(baseValue);
            }

            if (targetUnit is WeightUnit weight)
            {
                return weight.ConvertFromBaseUnit(baseValue);
            }

            if (targetUnit is VolumeUnit volume)
            {
                return volume.ConvertFromBaseUnit(baseValue);
            }

            if (targetUnit is TemperatureUnit temp)
            {
                // conversions for temperature units are defined in
                // TemperatureUnitHelper and handled similarly to the other
                // categories.
                return temp.ConvertFromBaseUnit(baseValue);
            }

            throw new InvalidOperationException("Unsupported unit type");
        }

        // ---------------- CORE ARITHMETIC HELPER ----------------
        private double PerformArithmetic(Quantity<U> other, ArithmeticOperation operation)
        {
            double base1 = ConvertToBase();
            double base2 = other.ConvertToBase();

            double result;

            if (operation == ArithmeticOperation.Add)
            {
                result = base1 + base2;
            }
            else if (operation == ArithmeticOperation.Subtract)
            {
                result = base1 - base2;
            }
            else if (operation == ArithmeticOperation.Divide)
            {
                if (base2 == 0)
                {
                    throw new ArithmeticException("Cannot divide by zero");
                }

                result = base1 / base2;
            }
            else
            {
                throw new ArgumentException("Invalid arithmetic operation");
            }

            return result;
        }

        // ---------------- ADD ----------------
        public Quantity<U> Add(Quantity<U> other)
        {
            ValidateArithmeticOperands(other, Unit, false);

            ValidateOperationSupport("Addition");
            return Add(other, Unit);
        }

        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            ValidateArithmeticOperands(other, targetUnit, true);

            double baseResult = PerformArithmetic(other, ArithmeticOperation.Add);

            double converted = ConvertBaseToTarget(baseResult, targetUnit);

            return new Quantity<U>(converted, targetUnit);
        }

        // ---------------- SUBTRACT ----------------
        public Quantity<U> Subtract(Quantity<U> other)
        {
            // mirror Add semantics: validate operands and operation support
            ValidateArithmeticOperands(other, Unit, false);
            ValidateOperationSupport("Subtraction");
            return Subtract(other, Unit);
        }

        public Quantity<U> Subtract(Quantity<U> other, U targetUnit)
        {
            ValidateArithmeticOperands(other, targetUnit, true);
            ValidateOperationSupport("Subtraction");

            double baseResult = PerformArithmetic(other, ArithmeticOperation.Subtract);

            double converted = ConvertBaseToTarget(baseResult, targetUnit);

            return new Quantity<U>(converted, targetUnit);
        }

        // ---------------- DIVIDE ----------------
        public double Divide(Quantity<U> other)
        {
            ValidateArithmeticOperands(other, default(U), false);
            ValidateOperationSupport("Division");

            return PerformArithmetic(other, ArithmeticOperation.Divide);
        }

        // ---------------- EQUALITY ----------------
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            Quantity<U> other = (Quantity<U>)obj;

            double base1 = ConvertToBase();
            double base2 = other.ConvertToBase();

            return Math.Abs(base1 - base2) < EPSILON;
        }
        public double ConvertTo(U targetUnit)
        {
            double baseValue = ConvertToBase();

            if (targetUnit is LengthUnit length)
            {
                return length.ConvertFromBaseUnit(baseValue);
            }
            if (targetUnit is WeightUnit weight)
            {
                return weight.ConvertFromBaseUnit(baseValue);
            }
            if (targetUnit is VolumeUnit volume)
            {
                return volume.ConvertFromBaseUnit(baseValue);
            }
            if (targetUnit is TemperatureUnit temp)
            {
                return temp.ConvertFromBaseUnit(baseValue);
            }
            throw new InvalidOperationException("Unsupported unit type");
        }
        public override int GetHashCode()
        {
            return ConvertToBase().GetHashCode();
        }

        //helper method for operation support -uc14
        private void ValidateOperationSupport(string operation)
        {
            if (Unit is TemperatureUnit tempUnit)
            {
                if (!tempUnit.SupportsArithmetic())
                {
                    tempUnit.ValidateOperationSupport(operation);
                }
            }
        }
        public override string ToString()
        {
            return $"{Value} {Unit}";
        }
    }
}