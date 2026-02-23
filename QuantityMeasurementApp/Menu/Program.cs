using QuantityMeasurementApp.App;
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
                Console.WriteLine("4 UC4");
                Console.WriteLine("5 Exit");
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
                        System.Console.WriteLine("Exitingg.");
                        break;
                    default:
                        Console.WriteLine("Invalid Choice");
                        break;
                }

            } while (choice != 5);
        }
    }
}