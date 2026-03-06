using QuantityMeasurementApp.App;
using QuantityMeasurementApp.Models;
namespace QuantityMeasurementApp
{
    class Program
    {
        public static void Main()
        {
            int choice;

            do
            {
                Console.WriteLine("\n===== Quantity Measurement Menu =====");
                Console.WriteLine("1 -> UC1 Feet Equality");
                Console.WriteLine("2 -> UC2 Inches Equality");
                Console.WriteLine("3 -> UC3 Generic Length Equality");
                Console.WriteLine("4 -> UC4 Extended Units");
                Console.WriteLine("5 -> UC5 Length Conversion");
                Console.WriteLine("6 -> UC6 Addition For two Lengths");
                Console.WriteLine("7 -> Exit");

                Console.Write("Enter Choice : ");
                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    // UC1
                    case 1:
                        QuantityMeasurementUtility.DemonstrateFeetEquality();
                        break;

                    // UC2
                    case 2:
                        QuantityMeasurementUtility.DemonstrateFeetEquality();
                        QuantityMeasurementUtility.DemonstrateInchesEquality();
                        break;

                    // UC3 
                    case 3:
                        QuantityLengthUtility.DemonstrateFeetEquality();
                        QuantityLengthUtility.DemonstrateInchesEquality();
                        QuantityLengthUtility.DemonstrateFeetInchesComparison();
                        break;

                    case 4:
                        QuantityExtendedLength.RunDemo();
                        break;
                    case 5:

                        QuantityMeasurementStatic.RunDemo();

                        break;
                    case 6:
                        Console.Write("Enter first value: ");
                        double v1 = double.Parse(Console.ReadLine());
                        Console.Write("Enter first unit (Feet/Inches/Yards/Centimeters): ");
                        Length.LengthUnit u1 = Enum.Parse<Length.LengthUnit>(Console.ReadLine());
                        Console.Write("Enter second value: ");
                        double v2 = double.Parse(Console.ReadLine());
                        Console.Write("Enter second unit: ");
                        Length.LengthUnit u2 = Enum.Parse<Length.LengthUnit>(Console.ReadLine());
                        var result = QuantityMeasurementStatic.demonstrateLengthAddition(v1, u1, v2, u2);
                        Console.WriteLine($"Result: {result}");
                        break;
                    case 7:
                        System.Console.WriteLine("Exitingg");
                        break;
                    default:
                        Console.WriteLine("Invalid Choice");
                        break;
                }

            } while (choice != 7);
        }
    }
}