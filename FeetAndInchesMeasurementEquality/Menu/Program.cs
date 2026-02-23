using FeetAndInchesMeasurementEquality.App;

namespace FeetAndInchesMeasurementEquality
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
                        QuantityMeasurementApp.DemonstrateFeetEquality();
                        break;

                    case 2:
                        QuantityMeasurementApp.DemonstrateFeetEquality();
                        QuantityMeasurementApp.DemonstrateInchesEquality();
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