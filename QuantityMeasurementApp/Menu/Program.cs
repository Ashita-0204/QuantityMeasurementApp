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
                Console.WriteLine("7 -> UC7 Addition With Target Unit");
                Console.WriteLine("8 -> UC9 Weight Operations");
                Console.WriteLine("9 -> UC10 Generic Quantity Operations");
                Console.WriteLine("10 -> UC11 Volume Operations");
                Console.WriteLine("11 -> UC12 Subtraction and Division");
                Console.WriteLine("12 -> Exit");
                Console.Write("Enter Choice : ");
                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 6:
                        Console.Write("Enter first value: ");
                        double v1 = double.Parse(Console.ReadLine());

                        Console.Write("Enter first unit (Feet/Inches/Yards/Centimeters): ");
                        LengthUnit u1 = Enum.Parse<LengthUnit>(Console.ReadLine());

                        Console.Write("Enter second value: ");
                        double v2 = double.Parse(Console.ReadLine());

                        Console.Write("Enter second unit: ");
                        LengthUnit u2 = Enum.Parse<LengthUnit>(Console.ReadLine());

                        var result = QuantityMeasurementStatic.demonstrateLengthAddition(v1, u1, v2, u2);

                        Console.WriteLine($"Result: {result}");
                        break;

                    case 7:
                        Console.Write("Enter first value: ");
                        double v1t = double.Parse(Console.ReadLine());

                        Console.Write("Enter first unit (Feet/Inches/Yards/Centimeters): ");
                        LengthUnit u1t = Enum.Parse<LengthUnit>(Console.ReadLine());

                        Console.Write("Enter second value: ");
                        double v2t = double.Parse(Console.ReadLine());

                        Console.Write("Enter second unit: ");
                        LengthUnit u2t = Enum.Parse<LengthUnit>(Console.ReadLine());

                        Console.Write("Enter target unit: ");
                        LengthUnit target = Enum.Parse<LengthUnit>(Console.ReadLine());

                        var result2 = QuantityMeasurementStatic.demonstrateLengthAdditionWithTargetUnit(v1t, u1t, v2t, u2t, target);

                        Console.WriteLine($"Result: {result2}");
                        break;
                    // -------- UC9 Weight Operations --------
                    case 8:

                        Console.WriteLine("\n--- Weight Operations ---");
                        Console.WriteLine("1 -> Equality");
                        Console.WriteLine("2 -> Conversion");
                        Console.WriteLine("3 -> Addition");
                        Console.WriteLine("4 -> Addition With Target Unit");

                        Console.Write("Select operation: ");
                        int weightChoice = int.Parse(Console.ReadLine());

                        switch (weightChoice)
                        {
                            case 1:
                                Console.Write("Enter first value: ");
                                double w1 = double.Parse(Console.ReadLine());

                                Console.Write("Enter first unit (Kilogram/Gram/Pound): ");
                                WeightUnit wu1 = Enum.Parse<WeightUnit>(Console.ReadLine());

                                Console.Write("Enter second value: ");
                                double w2 = double.Parse(Console.ReadLine());

                                Console.Write("Enter second unit: ");
                                WeightUnit wu2 = Enum.Parse<WeightUnit>(Console.ReadLine());

                                Weight weight1 = new Weight(w1, wu1);
                                Weight weight2 = new Weight(w2, wu2);

                                Console.WriteLine($"Equal: {weight1.Equals(weight2)}");
                                break;

                            case 2:
                                Console.Write("Enter value: ");
                                double val = double.Parse(Console.ReadLine());

                                Console.Write("Enter source unit (Kilogram/Gram/Pound): ");
                                WeightUnit from = Enum.Parse<WeightUnit>(Console.ReadLine());

                                Console.Write("Enter target unit: ");
                                WeightUnit to = Enum.Parse<WeightUnit>(Console.ReadLine());

                                Weight weight = new Weight(val, from);

                                Console.WriteLine($"Converted: {weight.ConvertTo(to)}");
                                break;

                            case 3:
                                Console.Write("Enter first value: ");
                                double wa1 = double.Parse(Console.ReadLine());

                                Console.Write("Enter first unit: ");
                                WeightUnit wua1 = Enum.Parse<WeightUnit>(Console.ReadLine());

                                Console.Write("Enter second value: ");
                                double wa2 = double.Parse(Console.ReadLine());

                                Console.Write("Enter second unit: ");
                                WeightUnit wua2 = Enum.Parse<WeightUnit>(Console.ReadLine());

                                Weight weightA = new Weight(wa1, wua1);
                                Weight weightB = new Weight(wa2, wua2);

                                Console.WriteLine($"Result: {weightA.Add(weightB)}");
                                break;

                            case 4:
                                Console.Write("Enter first value: ");
                                double wt1 = double.Parse(Console.ReadLine());

                                Console.Write("Enter first unit: ");
                                WeightUnit wut1 = Enum.Parse<WeightUnit>(Console.ReadLine());

                                Console.Write("Enter second value: ");
                                double wt2 = double.Parse(Console.ReadLine());

                                Console.Write("Enter second unit: ");
                                WeightUnit wut2 = Enum.Parse<WeightUnit>(Console.ReadLine());

                                Console.Write("Enter target unit: ");
                                WeightUnit wtTarget = Enum.Parse<WeightUnit>(Console.ReadLine());

                                Weight wA = new Weight(wt1, wut1);
                                Weight wB = new Weight(wt2, wut2);

                                Console.WriteLine($"Result: {wA.Add(wB, wtTarget)}");
                                break;
                        }

                        break;
                    // ---------------- UC10 ----------------
                    case 9:

                        Console.WriteLine("\n--- Generic Quantity Operations ---");
                        Console.WriteLine("1 -> Equality");
                        Console.WriteLine("2 -> Conversion");
                        Console.WriteLine("3 -> Addition");

                        int genChoice = int.Parse(Console.ReadLine());

                        switch (genChoice)
                        {
                            case 1:

                                var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
                                var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inches);

                                Console.WriteLine($"Equal: {q1.Equals(q2)}");
                                break;

                            case 2:

                                var q = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

                                double converted = q.ConvertTo(LengthUnit.Inches);

                                Console.WriteLine($"Converted: {converted} Inches");
                                break;

                            case 3:

                                var a = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
                                var b = new Quantity<LengthUnit>(12.0, LengthUnit.Inches);

                                var sum = a.Add(b, LengthUnit.Feet);

                                Console.WriteLine($"Sum: {sum}");
                                break;
                        }

                        break;
                    case 10:
                        // ---------------- UC11 VOLUME ----------------

                        Console.WriteLine("\n--- Volume Operations ---");
                        Console.WriteLine("1 -> Equality");
                        Console.WriteLine("2 -> Conversion");
                        Console.WriteLine("3 -> Addition");

                        int volChoice = int.Parse(Console.ReadLine());

                        if (volChoice == 1)
                        {
                            Console.Write("Enter first value: ");
                            double vA = double.Parse(Console.ReadLine());

                            Console.Write("Enter first unit (Litre/Millilitre/Gallon): ");
                            VolumeUnit vu1 = Enum.Parse<VolumeUnit>(Console.ReadLine(), true);

                            Console.Write("Enter second value: ");
                            double vB = double.Parse(Console.ReadLine());

                            Console.Write("Enter second unit: ");
                            VolumeUnit vu2 = Enum.Parse<VolumeUnit>(Console.ReadLine(), true);

                            var vol1 = new Quantity<VolumeUnit>(vA, vu1);
                            var vol2 = new Quantity<VolumeUnit>(vB, vu2);

                            Console.WriteLine($"Equal: {vol1.Equals(vol2)}");
                        }

                        else if (volChoice == 2)
                        {
                            Console.Write("Enter value: ");
                            double val = double.Parse(Console.ReadLine());

                            Console.Write("Enter source unit: ");
                            VolumeUnit from = Enum.Parse<VolumeUnit>(Console.ReadLine(), true);

                            Console.Write("Enter target unit: ");
                            VolumeUnit to = Enum.Parse<VolumeUnit>(Console.ReadLine(), true);

                            var vol = new Quantity<VolumeUnit>(val, from);

                            double converted = vol.ConvertTo(to);

                            Console.WriteLine($"Converted: {converted} {to}");
                        }

                        else if (volChoice == 3)
                        {
                            Console.Write("Enter first value: ");
                            double av1 = double.Parse(Console.ReadLine());

                            Console.Write("Enter first unit: ");
                            VolumeUnit au1 = Enum.Parse<VolumeUnit>(Console.ReadLine(), true);

                            Console.Write("Enter second value: ");
                            double av2 = double.Parse(Console.ReadLine());

                            Console.Write("Enter second unit: ");
                            VolumeUnit au2 = Enum.Parse<VolumeUnit>(Console.ReadLine(), true);

                            var volA = new Quantity<VolumeUnit>(av1, au1);
                            var volB = new Quantity<VolumeUnit>(av2, au2);
                            var volumeResult = volA.Add(volB);
                            Console.WriteLine($"Result: {volumeResult}"); Console.WriteLine($"Result: {volumeResult}");
                        }

                        break;
                    // ---------------- UC12 SUBTRACTION & DIVISION ----------------
                    case 11:

                        Console.WriteLine("\n--- UC12 Arithmetic Operations ---");
                        Console.WriteLine("1 -> Subtraction");
                        Console.WriteLine("2 -> Subtraction With Target Unit");
                        Console.WriteLine("3 -> Division");

                        int op = int.Parse(Console.ReadLine());

                        Console.Write("Enter first value: ");
                        double sv1 = double.Parse(Console.ReadLine());

                        Console.Write("Enter first unit (Feet/Inches/Yards/Centimeters): ");
                        LengthUnit su1 = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

                        Console.Write("Enter second value: ");
                        double sv2 = double.Parse(Console.ReadLine());

                        Console.Write("Enter second unit: ");
                        LengthUnit su2 = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

                        var qA = new Quantity<LengthUnit>(sv1, su1);
                        var qB = new Quantity<LengthUnit>(sv2, su2);

                        switch (op)
                        {
                            case 1:

                                var sub = qA.Subtract(qB);
                                Console.WriteLine($"Result: {sub}");
                                break;

                            case 2:

                                Console.Write("Enter target unit: ");
                                LengthUnit targett = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

                                var subTarget = qA.Subtract(qB, targett);
                                Console.WriteLine($"Result: {subTarget}");
                                break;

                            case 3:

                                double div = qA.Divide(qB);
                                Console.WriteLine($"Result: {div}");
                                break;
                        }

                        break;
                    case 12:
                        Console.WriteLine("Exiting");
                        break;
                }

            } while (choice != 11);
        }
    }
}