using QuantityMeasurementApp.Services;
using QuantityMeasurementApp.Exceptions;

namespace QuantityMeasurementApp.App
{
    public class QuantityMeasurementUtility
    {
        private static QuantityMeasurementService service = new QuantityMeasurementService();

        // UC1 Static Method
        public static void DemonstrateFeetEquality()
        {
            try
            {
                Console.Write("Enter First Feet Value : ");
                double feetOne = double.Parse(Console.ReadLine());
                Console.Write("Enter Second Feet Value : ");
                double feetTwo = double.Parse(Console.ReadLine());
                bool result = service.AreEqual(feetOne, feetTwo);
                Console.WriteLine("Feet Equality Result : " + result);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid Numeric Input");
            }
            catch (QuantityMeasurementException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // UC2 Static Method
        public static void DemonstrateInchesEquality()
        {
            try
            {
                Console.Write("Enter First Inches Value : ");
                double inchOne = double.Parse(Console.ReadLine());
                Console.Write("Enter Second Inches Value : ");
                double inchTwo = double.Parse(Console.ReadLine());
                bool result = service.AreInchesEqual(inchOne, inchTwo);
                Console.WriteLine("Inches Equality Result : " + result);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid Numeric Input");
            }
            catch (QuantityMeasurementException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}