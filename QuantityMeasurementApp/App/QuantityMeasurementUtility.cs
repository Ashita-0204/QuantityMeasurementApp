using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp.App
{
    public class QuantityMeasurementUtility
    {
        private static QuantityMeasurementService service = new QuantityMeasurementService();

        public static void DemonstrateFeetEquality()
        {
            Console.Write("Enter First Feet Value : ");
            double f1 = double.Parse(Console.ReadLine());
            Console.Write("Enter Second Feet Value : ");
            double f2 = double.Parse(Console.ReadLine());
            bool result = service.AreEqual(f1, f2);
            Console.WriteLine("Feet Equality Result : " + result);
        }
    }
}