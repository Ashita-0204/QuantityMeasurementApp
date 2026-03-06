using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.App
{
    public static class QuantityOperations
    {
        public static bool DemonstrateEquality<U>(Quantity<U> q1, Quantity<U> q2)
            where U : struct, Enum
        {
            return q1.Equals(q2);
        }

        public static Quantity<U> DemonstrateConversion<U>(Quantity<U> q, U targetUnit)
            where U : struct, Enum
        {
            double value = q.ConvertTo(targetUnit);
            return new Quantity<U>(value, targetUnit);
        }

        public static Quantity<U> DemonstrateAddition<U>(Quantity<U> q1, Quantity<U> q2)
            where U : struct, Enum
        {
            return q1.Add(q2);
        }

        public static Quantity<U> DemonstrateAddition<U>(Quantity<U> q1, Quantity<U> q2, U targetUnit)
            where U : struct, Enum
        {
            return q1.Add(q2, targetUnit);
        }

        public static Quantity<U> DemonstrateSubtraction<U>(Quantity<U> q1, Quantity<U> q2)
    where U : Enum
        {
            return q1.Subtract(q2);
        }

        public static Quantity<U> DemonstrateSubtraction<U>(Quantity<U> q1, Quantity<U> q2, U targetUnit)
            where U : Enum
        {
            return q1.Subtract(q2, targetUnit);
        }

        public static double DemonstrateDivision<U>(Quantity<U> q1, Quantity<U> q2)
            where U : Enum
        {
            return q1.Divide(q2);
        }
    }
}