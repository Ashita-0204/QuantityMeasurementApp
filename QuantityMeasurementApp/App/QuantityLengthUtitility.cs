using QuantityMeasurementApp.Services;
using QuantityMeasurementApp.Models
;

namespace QuantityMeasurementApp.App
{
    public class QuantityLengthUtility
    {
        private static QuantityMeasurementService service = new QuantityMeasurementService();

        //UC3 Implementation
        public static bool DemonstrateLengthEquality(Length lenOne, Length lenTwo)
        {
            return service.AreEqual(lenOne, lenTwo);
        }

        public static void DemonstrateFeetEquality()
        {
            var feetOne = new Length(1, LengthUnit.Feet);
            var feetTwo = new Length(1, LengthUnit.Feet);

            Console.WriteLine($"Feet Equal : {feetOne.Equals(feetTwo)}");
        }

        public static void DemonstrateInchesEquality()
        {
            var inchOne = new Length(1, LengthUnit.Inches);
            var inchTwo = new Length(1, LengthUnit.Inches);
            Console.WriteLine($"Inches Equal : {inchOne.Equals(inchTwo)}");
        }

        public static void DemonstrateFeetInchesComparison()
        {
            var feet = new Length(1, LengthUnit.Feet);
            var inches = new Length(12, LengthUnit.Inches);
            Console.WriteLine($"Feet vs Inches Equal : {feet.Equals(inches)}");
        }

    }
}