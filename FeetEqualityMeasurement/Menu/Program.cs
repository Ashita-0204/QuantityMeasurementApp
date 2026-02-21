using FeetEqualityMeasurement.Services;
using FeetEqualityMeasurement.Exceptions;

class Program
{
    static void Main()
    {
        try
        {
            QuantityMeasurementService service = new QuantityMeasurementService();

            Console.Write("Enter First Feet Value : ");
            double value1 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter Second Feet Value : ");
            double value2 = Convert.ToDouble(Console.ReadLine());

            bool result = service.AreEqual(value1, value2);

            Console.WriteLine("Equality Result : " + result);
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid Input. Please enter numeric value.");
        }
        catch (QuantityMeasurementException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}