using FeetAndInchesMeasurementEquality.Services;
using FeetAndInchesMeasurementEquality.Exceptions;

class Program
{
    static void Main()
    {
        try
        {
            QuantityMeasurementService service = new QuantityMeasurementService();

            // Feet Comparison(UC1)
            Console.Write("Enter First Feet Value : ");
            double feet1 = Convert.ToDouble(Console.ReadLine());
            Console.Write("Enter Second Feet Value : ");
            double feet2 = Convert.ToDouble(Console.ReadLine());

            bool feetResult = service.AreEqual(feet1, feet2);

            Console.WriteLine("Feet Equality Result : " + feetResult);

            // Inches Comparison(UC2)
            Console.Write("Enter First Inches Value : ");
            double inch1 = Convert.ToDouble(Console.ReadLine());
            Console.Write("Enter Second Inches Value : ");
            double inch2 = Convert.ToDouble(Console.ReadLine());

            bool inchResult = service.AreInchesEqual(inch1, inch2);

            Console.WriteLine("Inches Equality Result : " + inchResult);
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid Input. Enter numeric value.");
        }
        catch (QuantityMeasurementException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}