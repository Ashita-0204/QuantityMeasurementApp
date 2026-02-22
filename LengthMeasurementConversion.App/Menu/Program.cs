using LengthMeasurementConversion.App.Services;
using LengthMeasurementConversion.App.Exceptions;
using LengthMeasurementConversion.App.Models;
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
        //UC4 Implementation
        try
        {
            Console.WriteLine("1 Yard == 3 Feet ?");

            bool result1 = service.AreLengthEquall(1, LengthUnit.Yards, 3, LengthUnit.Feet);
            Console.WriteLine(result1);

            Console.WriteLine("1 cm == 0.393701 inch ?");
            bool result2 = service.AreLengthEquall(1, LengthUnit.Centimeters, 0.393701, LengthUnit.Inches);
            Console.WriteLine(result2);
        }
        catch (QuantityMeasurementException ex)
        {
            Console.WriteLine("Error : " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected Error : " + ex.Message);
        }

        //UC5 Implementation
        // UC5 Implementation

        try
        {
            Console.WriteLine("1 Feet -> Inches");
            Console.WriteLine(service.ConvertLength(1, LengthUnit.Feet, LengthUnit.Inches));

            Console.WriteLine("3 Yard -> Feet");
            Console.WriteLine(service.ConvertLength(3, LengthUnit.Yards, LengthUnit.Feet));

            Console.WriteLine("2.54 cm -> Inches");
            Console.WriteLine(service.ConvertLength(2.54, LengthUnit.Centimeters, LengthUnit.Inches));
        }
        catch (QuantityMeasurementException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}