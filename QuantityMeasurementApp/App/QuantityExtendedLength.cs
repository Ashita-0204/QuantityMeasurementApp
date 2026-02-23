using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.App
{
    public class QuantityExtendedLength
    {
        public static void RunDemo()
        {
            DemonstrateFeetEquality();
            DemonstrateInchesEquality();
            DemonstrateFeetInchesComparison();
        }
        public static bool DemonstrateLengthEquality(Length lenOne, Length lenTwo)
        {
            return lenOne.Equals(lenTwo);
        }

        public static void DemonstrateFeetEquality()
        {
            var feetOne = new Length(1, Length.LengthUnit.Feet);
            var feetTwo = new Length(1, Length.LengthUnit.Feet);
            Console.WriteLine($"Feet Equal : {feetOne.Equals(feetTwo)}");
        }

        public static void DemonstrateInchesEquality()
        {
            var inchOne = new Length(1, Length.LengthUnit.Inches);
            var inchTwo = new Length(1, Length.LengthUnit.Inches);
            Console.WriteLine($"Inches Equal : {inchOne.Equals(inchTwo)}");
        }

        public static void DemonstrateFeetInchesComparison()
        {
            var feet = new Length(1, Length.LengthUnit.Feet);
            var inches = new Length(12, Length.LengthUnit.Inches);
            Console.WriteLine($"Feet vs Inches Equal : {feet.Equals(inches)}");
        }
    }
}