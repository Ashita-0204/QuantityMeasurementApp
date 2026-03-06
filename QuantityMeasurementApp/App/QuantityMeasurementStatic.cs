using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.App
{
    public class QuantityMeasurementStatic
    {
        public static bool demonstrateLengthEquality(Length l1, Length l2)
        {
            return l1.Equals(l2);
        }

        public static bool demonstrateLengthComparison(double v1, LengthUnit u1, double v2, LengthUnit u2)
        {
            Length l1 = new Length(v1, u1);
            Length l2 = new Length(v2, u2);

            return l1.Equals(l2);
        }

        public static Length demonstrateLengthConversion(double value, LengthUnit from, LengthUnit to)
        {
            Length length = new Length(value, from);
            return length.ConvertTo(to);
        }

        public static Length demonstrateLengthConversion(Length length, LengthUnit toUnit)
        {
            return length.ConvertTo(toUnit);
        }

        public static void RunDemo()
        {
            Console.WriteLine("\n--- UC5 Conversion Demo ---");

            var result = demonstrateLengthConversion(3, LengthUnit.Feet, LengthUnit.Inches);
            Console.WriteLine($"Converted: {result}");

            Console.WriteLine("\n--- UC6 Addition Demo ---");
            var addResult = demonstrateLengthAddition(1, LengthUnit.Feet, 12, LengthUnit.Inches);
            Console.WriteLine($"Addition Result: {addResult}");
        }

        public static Length demonstrateLengthAddition(double v1, LengthUnit u1, double v2, LengthUnit u2)
        {
            Length l1 = new Length(v1, u1);
            Length l2 = new Length(v2, u2);

            return l1.Add(l2);
        }

        public static Length demonstrateLengthAdditionWithTargetUnit(double v1, LengthUnit u1, double v2, LengthUnit u2, LengthUnit targetUnit)
        {
            Length l1 = new Length(v1, u1);
            Length l2 = new Length(v2, u2);

            return l1.Add(l2, targetUnit);
        }
    }
}