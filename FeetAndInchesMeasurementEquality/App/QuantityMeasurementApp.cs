using FeetAndInchesMeasurementEquality.Services;
using FeetAndInchesMeasurementEquality.Exceptions;

namespace FeetAndInchesMeasurementEquality.App
{
    public class QuantityMeasurementApp
    {
        private static QuantityMeasurementService service =
            new QuantityMeasurementService();

        // UC1 Static Method
        public static void DemonstrateFeetEquality()
        {
            try
            {
                Console.Write("Enter First Feet Value : ");
                double f1 = double.Parse(Console.ReadLine());
                Console.Write("Enter Second Feet Value : ");
                double f2 = double.Parse(Console.ReadLine());
                bool result = service.AreEqual(f1, f2);
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
                double i1 = double.Parse(Console.ReadLine());
                Console.Write("Enter Second Inches Value : ");
                double i2 = double.Parse(Console.ReadLine());
                bool result = service.AreInchesEqual(i1, i2);
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