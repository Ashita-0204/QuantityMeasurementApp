using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    // compatibility wrapper needed by legacy tests
    public class QuantityMeasurementService
    {
        public bool AreEqual(double valueOne, double valueTwo)
            => new Feet(valueOne).Equals(new Feet(valueTwo));

        public bool AreInchesEqual(double valueOne, double valueTwo)
            => new Inches(valueOne).Equals(new Inches(valueTwo));

        public bool AreEqual(Length lenOne, Length lenTwo)
            => lenOne.Equals(lenTwo);
    }
}

namespace QuantityMeasurementApp.App
{
    public static class QuantityMeasurementStatic
    {
        public static Length demonstrateLengthConversion(double value, LengthUnit from, LengthUnit to)
        {
            double converted = Length.Convert(value, from, to);
            return new Length(converted, to);
        }

        public static Length demonstrateLengthConversion(Length input, LengthUnit to)
        {
            return input.ConvertTo(to);
        }

        public static Length demonstrateLengthAddition(double v1, LengthUnit u1, double v2, LengthUnit u2)
        {
            var l1 = new Length(v1, u1);
            var l2 = new Length(v2, u2);
            return l1.Add(l2);
        }
    }

    public static class QuantityOperations
    {
        public static bool DemonstrateEquality<U>(Quantity<U> a, Quantity<U> b) where U : Enum
            => a.Equals(b);

        public static Quantity<U> DemonstrateConversion<U>(Quantity<U> q, U target) where U : Enum
        {
            double v = q.ConvertTo(target);
            return new Quantity<U>(v, target);
        }

        public static Quantity<U> DemonstrateAddition<U>(Quantity<U> a, Quantity<U> b, U target) where U : Enum
            => a.Add(b, target);
    }
}