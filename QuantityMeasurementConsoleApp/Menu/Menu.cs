using QuantityMeasurementRepository;
using QuantityMeasurementBusinessLayer;
using QuantityMeasurementApp.Models;
using QuantityMeasurementModel.DTOs;

public class Menu : IMenu
{
    public void ShowMenu()
    {
        int choice;

        var repository = QuantityMeasurementCacheRepository.Instance;
        var service = new QuantityMeasurementServiceImpl(repository);

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
            Console.WriteLine("12 -> UC14 Temperature Operations");
            Console.WriteLine("13 -> Exit");
            Console.Write("Enter Choice : ");

            choice = int.Parse(Console.ReadLine()!);

            switch (choice)
            {
                case 6:

                    Console.Write("Enter first value: ");
                    double firstValue = double.Parse(Console.ReadLine()!);

                    Console.Write("Enter first unit (Feet/Inches/Yards/Centimeters): ");
                    LengthUnit firstUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!);

                    Console.Write("Enter second value: ");
                    double secondValue = double.Parse(Console.ReadLine()!);

                    Console.Write("Enter second unit: ");
                    LengthUnit secondUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!);

                    var quantityOne = new QuantityDTO(firstValue, firstUnit.ToString(), "Length");
                    var quantityTwo = new QuantityDTO(secondValue, secondUnit.ToString(), "Length");

                    var response = service.Add(quantityOne, quantityTwo);

                    if (response.Success && response.Result != null)
                        Console.WriteLine($"Result: {response.Result.Value} {response.Result.Unit}");
                    else
                        Console.WriteLine(response.ErrorMessage);

                    break;

                case 7:

                    Console.Write("Enter first value: ");
                    double firstNumber = double.Parse(Console.ReadLine()!);

                    Console.Write("Enter first unit: ");
                    LengthUnit firstLengthUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!);

                    Console.Write("Enter second value: ");
                    double secondNumber = double.Parse(Console.ReadLine()!);

                    Console.Write("Enter second unit: ");
                    LengthUnit secondLengthUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!);

                    Console.Write("Enter target unit: ");
                    LengthUnit targetUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!);

                    var quantityA = new QuantityDTO(firstNumber, firstLengthUnit.ToString(), "Length");
                    var quantityB = new QuantityDTO(secondNumber, secondLengthUnit.ToString(), "Length");

                    var result = service.Add(quantityA, quantityB, targetUnit.ToString());

                    if (result.Success && result.Result != null)
                        Console.WriteLine($"Result: {result.Result.Value} {result.Result.Unit}");
                    else
                        Console.WriteLine(result.ErrorMessage);

                    break;

                case 8:

                    Console.WriteLine("\n--- Weight Operations ---");
                    Console.WriteLine("1 -> Equality");
                    Console.WriteLine("2 -> Conversion");
                    Console.WriteLine("3 -> Addition");
                    Console.WriteLine("4 -> Addition With Target Unit");

                    Console.Write("Select operation: ");
                    int weightOperation = int.Parse(Console.ReadLine()!);

                    switch (weightOperation)
                    {
                        case 1:

                            Console.Write("Enter first value: ");
                            double firstWeightValue = double.Parse(Console.ReadLine()!);

                            Console.Write("Enter first unit (Kilogram/Gram/Pound): ");
                            WeightUnit firstWeightUnit = Enum.Parse<WeightUnit>(Console.ReadLine()!);

                            Console.Write("Enter second value: ");
                            double secondWeightValue = double.Parse(Console.ReadLine()!);

                            Console.Write("Enter second unit: ");
                            WeightUnit secondWeightUnit = Enum.Parse<WeightUnit>(Console.ReadLine()!);

                            Weight weightOne = new Weight(firstWeightValue, firstWeightUnit);
                            Weight weightTwo = new Weight(secondWeightValue, secondWeightUnit);

                            Console.WriteLine($"Equal: {weightOne.Equals(weightTwo)}");

                            break;

                        case 2:

                            Console.Write("Enter value: ");
                            double weightValue = double.Parse(Console.ReadLine()!);

                            Console.Write("Enter source unit: ");
                            WeightUnit sourceWeightUnit = Enum.Parse<WeightUnit>(Console.ReadLine()!);

                            Console.Write("Enter target unit: ");
                            WeightUnit targetWeightUnit = Enum.Parse<WeightUnit>(Console.ReadLine()!);

                            Weight weight = new Weight(weightValue, sourceWeightUnit);

                            Console.WriteLine($"Converted: {weight.ConvertTo(targetWeightUnit)}");

                            break;

                        case 3:

                            Console.Write("Enter first value: ");
                            double weightAValue = double.Parse(Console.ReadLine()!);

                            Console.Write("Enter first unit: ");
                            WeightUnit weightAUnit = Enum.Parse<WeightUnit>(Console.ReadLine()!);

                            Console.Write("Enter second value: ");
                            double weightBValue = double.Parse(Console.ReadLine()!);

                            Console.Write("Enter second unit: ");
                            WeightUnit weightBUnit = Enum.Parse<WeightUnit>(Console.ReadLine()!);

                            Weight weightA = new Weight(weightAValue, weightAUnit);
                            Weight weightB = new Weight(weightBValue, weightBUnit);

                            Console.WriteLine($"Result: {weightA.Add(weightB)}");

                            break;

                        case 4:

                            Console.Write("Enter first value: ");
                            double weightValueA = double.Parse(Console.ReadLine()!);

                            Console.Write("Enter first unit: ");
                            WeightUnit weightUnitA = Enum.Parse<WeightUnit>(Console.ReadLine()!);

                            Console.Write("Enter second value: ");
                            double weightValueB = double.Parse(Console.ReadLine()!);

                            Console.Write("Enter second unit: ");
                            WeightUnit weightUnitB = Enum.Parse<WeightUnit>(Console.ReadLine()!);

                            Console.Write("Enter target unit: ");
                            WeightUnit weightTargetUnit = Enum.Parse<WeightUnit>(Console.ReadLine()!);

                            Weight weightObjA = new Weight(weightValueA, weightUnitA);
                            Weight weightObjB = new Weight(weightValueB, weightUnitB);

                            Console.WriteLine($"Result: {weightObjA.Add(weightObjB, weightTargetUnit)}");

                            break;
                    }

                    break;

                case 9:

                    Console.WriteLine("\n--- Generic Quantity Operations ---");
                    Console.WriteLine("1 -> Equality");
                    Console.WriteLine("2 -> Conversion");
                    Console.WriteLine("3 -> Addition");

                    int genericOperation = int.Parse(Console.ReadLine()!);

                    switch (genericOperation)
                    {
                        case 1:

                            var quantity1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
                            var quantity2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inches);

                            Console.WriteLine($"Equal: {quantity1.Equals(quantity2)}");

                            break;

                        case 2:

                            var quantity = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
                            double convertedValue = quantity.ConvertTo(LengthUnit.Inches);

                            Console.WriteLine($"Converted: {convertedValue} Inches");

                            break;

                        case 3:

                            var firstQuantity = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
                            var secondQuantity = new Quantity<LengthUnit>(12.0, LengthUnit.Inches);

                            var sum = firstQuantity.Add(secondQuantity, LengthUnit.Feet);

                            Console.WriteLine($"Sum: {sum}");

                            break;
                    }

                    break;

                case 13:
                    Console.WriteLine("Exiting");
                    break;
            }

        } while (choice != 13);
    }
}