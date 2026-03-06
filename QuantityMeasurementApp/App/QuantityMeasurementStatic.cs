using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.App
{
    public class QuantityMeasurementStatic
    {
        // Equality demonstration
        public static bool demonstrateLengthEquality(
            Length l1,
            Length l2)
        {
            return l1.Equals(l2);
        }

        // Comparison using raw values
        public static bool demonstrateLengthComparison(
            double v1,
            Length.LengthUnit u1,
            double v2,
            Length.LengthUnit u2)
        {
            Length l1 = new Length(v1, u1);
            Length l2 = new Length(v2, u2);

            return l1.Equals(l2);
        }

        // -------- UC5 Conversion (Overload 1) --------

        public static Length demonstrateLengthConversion(
            double value,
            Length.LengthUnit from,
            Length.LengthUnit to)
        {
            Length length = new Length(value, from);
            return length.ConvertTo(to);
        }

        // -------- UC5 Conversion (Overload 2) --------

        public static Length demonstrateLengthConversion(
            Length length,
            Length.LengthUnit toUnit)
        {
            return length.ConvertTo(toUnit);
        }

        // Demo for menu
        public static void RunDemo()
        {
            Console.WriteLine("\n--- UC5 Conversion Demo ---");

            var result = demonstrateLengthConversion(3, Length.LengthUnit.Feet, Length.LengthUnit.Inches);
            Console.WriteLine($"Converted: {result}");
            Console.WriteLine("\n--- UC6 Addition Demo ---");
            var addResult = demonstrateLengthAddition(1, Length.LengthUnit.Feet, 12, Length.LengthUnit.Inches);
            Console.WriteLine($"Addition Result: {addResult}");
        }

        // -------- UC6 Addition --------

        public static Length demonstrateLengthAddition(double v1, Length.LengthUnit u1, double v2, Length.LengthUnit u2)
        {
            Length l1 = new Length(v1, u1);
            Length l2 = new Length(v2, u2);

            return l1.Add(l2);
        }
    }
}