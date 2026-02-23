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
                Console.WriteLine("3 -> Exit");
                Console.Write("Enter Choice : ");
                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        QuantityMeasurementUtility.DemonstrateFeetEquality();
                        break;

                    case 2:
                        QuantityMeasurementUtility.DemonstrateFeetEquality();
                        QuantityMeasurementUtility.DemonstrateInchesEquality();
                        break;

                    case 3:
                        System.Console.WriteLine("Exiting..");
                        break;

                    default:
                        Console.WriteLine("Invalid Choice");
                        break;
                }
            } while (choice != 3);
        }
    }
}