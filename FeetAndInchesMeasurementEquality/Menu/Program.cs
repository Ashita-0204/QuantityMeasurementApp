using FeetAndInchesMeasurementEquality.Services;
using FeetAndInchesMeasurementEquality.Exceptions;
using FeetAndInchesMeasurementEquality.Models;
class Program
{
    static void Main()
    {
        QuantityMeasurementService service = new QuantityMeasurementService();

        try
        {

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

        //UC3 Implementation
        try
        {
            bool result3 = service.AreLengthEqual(1.0, LengthUnit.Feet, 12.0, LengthUnit.Inches);
            Console.WriteLine("1 Feet == 12 Inches ? : " + result3);
        }
        catch (QuantityMeasurementException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}