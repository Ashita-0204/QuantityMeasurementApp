using QuantityMeasurementRepository;
using QuantityMeasurementBusinessLayer;
using QuantityMeasurementApp.Models;
using QuantityMeasurementModel.DTOs;

public class Menu : IMenu
{
    private IQuantityMeasurementService _service = null!;

    public void ShowMenu(IQuantityMeasurementRepository repository)
    {
        _service = new QuantityMeasurementServiceImpl(repository);
        int choice;

        do
        {
            Console.WriteLine("\n===== Quantity Measurement Menu =====");
            Console.WriteLine("1  -> UC1  Feet Equality");
            Console.WriteLine("2  -> UC2  Inches Equality");
            Console.WriteLine("3  -> UC3  Generic Length Equality");
            Console.WriteLine("4  -> UC4  Extended Units Equality");
            Console.WriteLine("5  -> UC5  Length Conversion");
            Console.WriteLine("6  -> UC6  Addition For Two Lengths");
            Console.WriteLine("7  -> UC7  Addition With Target Unit");
            Console.WriteLine("8  -> UC9  Weight Operations");
            Console.WriteLine("9  -> UC10 Generic Quantity Operations");
            Console.WriteLine("10 -> UC11 Volume Operations");
            Console.WriteLine("11 -> UC12 Subtraction and Division");
            Console.WriteLine("12 -> UC14 Temperature Operations");
            Console.WriteLine("13 -> Exit");
            Console.Write("Enter Choice: ");

            if (!int.TryParse(Console.ReadLine(), out choice)) { choice = 0; continue; }

            try
            {
                HandleChoice(choice);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[ERROR] {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"[INNER] {ex.InnerException.Message}");
                Console.ResetColor();
            }

        } while (choice != 13);
    }

    private void HandleChoice(int choice)
    {
        switch (choice)
        {
            case 1:
                {
                    Console.Write("Enter first value (Feet): ");
                    double v1 = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter second value (Feet): ");
                    double v2 = double.Parse(Console.ReadLine()!);
                    Print(Run(() => _service.Compare(
                        new QuantityDTO(v1, "Feet", "Length"),
                        new QuantityDTO(v2, "Feet", "Length"))));
                    break;
                }
            case 2:
                {
                    Console.Write("Enter value in Feet: ");
                    double v1 = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter value in Inches: ");
                    double v2 = double.Parse(Console.ReadLine()!);
                    Print(Run(() => _service.Compare(
                        new QuantityDTO(v1, "Feet", "Length"),
                        new QuantityDTO(v2, "Inches", "Length"))));
                    break;
                }
            case 3:
            case 4:
                {
                    Console.WriteLine("Units: Feet / Inches / Yards / Centimeters");
                    Console.Write("Enter first value: ");
                    double v1 = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter first unit: ");
                    string u1 = Console.ReadLine()!.Trim();
                    Console.Write("Enter second value: ");
                    double v2 = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter second unit: ");
                    string u2 = Console.ReadLine()!.Trim();
                    Print(Run(() => _service.Compare(
                        new QuantityDTO(v1, u1, "Length"),
                        new QuantityDTO(v2, u2, "Length"))));
                    break;
                }
            case 5:
                {
                    Console.Write("Enter value: ");
                    double v = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter source unit (Feet/Inches/Yards/Centimeters): ");
                    string src = Console.ReadLine()!.Trim();
                    Console.Write("Enter target unit: ");
                    string tgt = Console.ReadLine()!.Trim();
                    Print(Run(() => _service.Convert(
                        new QuantityDTO(v, src, "Length"), tgt)));
                    break;
                }
            case 6:
                {
                    Console.Write("Enter first value: ");
                    double v1 = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter first unit (Feet/Inches/Yards/Centimeters): ");
                    string u1 = Console.ReadLine()!.Trim();
                    Console.Write("Enter second value: ");
                    double v2 = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter second unit: ");
                    string u2 = Console.ReadLine()!.Trim();
                    Print(Run(() => _service.Add(
                        new QuantityDTO(v1, u1, "Length"),
                        new QuantityDTO(v2, u2, "Length"))));
                    break;
                }
            case 7:
                {
                    Console.Write("Enter first value: ");
                    double v1 = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter first unit: ");
                    string u1 = Console.ReadLine()!.Trim();
                    Console.Write("Enter second value: ");
                    double v2 = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter second unit: ");
                    string u2 = Console.ReadLine()!.Trim();
                    Console.Write("Enter target unit: ");
                    string tgt = Console.ReadLine()!.Trim();
                    Print(Run(() => _service.Add(
                        new QuantityDTO(v1, u1, "Length"),
                        new QuantityDTO(v2, u2, "Length"), tgt)));
                    break;
                }
            case 8:
                {
                    Console.WriteLine("\n--- Weight Operations ---");
                    Console.WriteLine("1 -> Compare  2 -> Convert  3 -> Add  4 -> Subtract");
                    Console.Write("Select: ");
                    int op = int.Parse(Console.ReadLine()!);

                    Console.Write("Enter first value: ");
                    double v1 = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter first unit (Kilogram/Gram/Pound): ");
                    string u1 = Console.ReadLine()!.Trim();

                    if (op == 2)
                    {
                        Console.Write("Enter target unit: ");
                        string tgt = Console.ReadLine()!.Trim();
                        Print(Run(() => _service.Convert(new QuantityDTO(v1, u1, "Weight"), tgt)));
                        break;
                    }

                    Console.Write("Enter second value: ");
                    double v2 = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter second unit: ");
                    string u2 = Console.ReadLine()!.Trim();

                    var w1 = new QuantityDTO(v1, u1, "Weight");
                    var w2 = new QuantityDTO(v2, u2, "Weight");
                    Print(Run(() => op switch
                    {
                        1 => _service.Compare(w1, w2),
                        3 => _service.Add(w1, w2),
                        4 => _service.Subtract(w1, w2),
                        _ => throw new Exception("Invalid option")
                    }));
                    break;
                }
            case 9:
                {
                    Console.WriteLine("\n--- Generic Quantity Operations ---");
                    Console.WriteLine("1 -> Compare  2 -> Convert  3 -> Add");
                    Console.Write("Select: ");
                    int op = int.Parse(Console.ReadLine()!);

                    Console.Write("Category (Length/Weight/Volume): ");
                    string cat = Console.ReadLine()!.Trim();
                    Console.Write("Enter first value: ");
                    double v1 = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter first unit: ");
                    string u1 = Console.ReadLine()!.Trim();

                    if (op == 2)
                    {
                        Console.Write("Enter target unit: ");
                        string tgt = Console.ReadLine()!.Trim();
                        Print(Run(() => _service.Convert(new QuantityDTO(v1, u1, cat), tgt)));
                        break;
                    }

                    Console.Write("Enter second value: ");
                    double v2 = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter second unit: ");
                    string u2 = Console.ReadLine()!.Trim();
                    var q1 = new QuantityDTO(v1, u1, cat);
                    var q2 = new QuantityDTO(v2, u2, cat);
                    Print(Run(() => op == 1 ? _service.Compare(q1, q2) : _service.Add(q1, q2)));
                    break;
                }
            case 10:
                {
                    Console.WriteLine("\n--- Volume Operations ---");
                    Console.WriteLine("1 -> Compare  2 -> Convert  3 -> Add");
                    Console.Write("Select: ");
                    int op = int.Parse(Console.ReadLine()!);

                    Console.Write("Enter first value: ");
                    double v1 = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter first unit (Litre/Millilitre/Gallon): ");
                    string u1 = Console.ReadLine()!.Trim();

                    if (op == 2)
                    {
                        Console.Write("Enter target unit: ");
                        string tgt = Console.ReadLine()!.Trim();
                        Print(Run(() => _service.Convert(new QuantityDTO(v1, u1, "Volume"), tgt)));
                        break;
                    }

                    Console.Write("Enter second value: ");
                    double v2 = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter second unit: ");
                    string u2 = Console.ReadLine()!.Trim();
                    var vol1 = new QuantityDTO(v1, u1, "Volume");
                    var vol2 = new QuantityDTO(v2, u2, "Volume");
                    Print(Run(() => op == 1 ? _service.Compare(vol1, vol2) : _service.Add(vol1, vol2)));
                    break;
                }
            case 11:
                {
                    Console.WriteLine("\n--- Subtraction and Division ---");
                    Console.WriteLine("1 -> Subtract  2 -> Divide");
                    Console.Write("Select: ");
                    int op = int.Parse(Console.ReadLine()!);

                    Console.Write("Category (Length/Weight/Volume): ");
                    string cat = Console.ReadLine()!.Trim();
                    Console.Write("Enter first value: ");
                    double v1 = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter first unit: ");
                    string u1 = Console.ReadLine()!.Trim();
                    Console.Write("Enter second value: ");
                    double v2 = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter second unit: ");
                    string u2 = Console.ReadLine()!.Trim();

                    var s1 = new QuantityDTO(v1, u1, cat);
                    var s2 = new QuantityDTO(v2, u2, cat);
                    Print(Run(() => op == 1 ? _service.Subtract(s1, s2) : _service.Divide(s1, s2)));
                    break;
                }
            case 12:
                {
                    Console.WriteLine("\n--- Temperature Operations ---");
                    Console.WriteLine("1 -> Compare  2 -> Convert");
                    Console.Write("Select: ");
                    int op = int.Parse(Console.ReadLine()!);

                    Console.Write("Enter value: ");
                    double v1 = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter unit (Celsius/Fahrenheit/Kelvin): ");
                    string u1 = Console.ReadLine()!.Trim();

                    if (op == 2)
                    {
                        Console.Write("Enter target unit: ");
                        string tgt = Console.ReadLine()!.Trim();
                        Print(Run(() => _service.Convert(new QuantityDTO(v1, u1, "Temperature"), tgt)));
                        break;
                    }

                    Console.Write("Enter second value: ");
                    double v2 = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter second unit: ");
                    string u2 = Console.ReadLine()!.Trim();
                    Print(Run(() => _service.Compare(
                        new QuantityDTO(v1, u1, "Temperature"),
                        new QuantityDTO(v2, u2, "Temperature"))));
                    break;
                }
            case 13:
                Console.WriteLine("Exiting...");
                break;
            default:
                Console.WriteLine("Invalid choice. Enter 1-13.");
                break;
        }
    }

    // ── Wraps a service call — catches DB/save exceptions and shows them clearly ──
    private static QuantityResponseDTO Run(Func<QuantityResponseDTO> call)
    {
        try
        {
            return call();
        }
        catch (Exception ex)
        {
            // Surface the full DB error so nothing is silently swallowed
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[DB ERROR] {ex.Message}");
            if (ex.InnerException != null)
                Console.WriteLine($"[DB INNER] {ex.InnerException.Message}");
            Console.ResetColor();
            throw; // re-throw so outer catch in ShowMenu also logs it
        }
    }

    private static void Print(QuantityResponseDTO r)
    {
        if (!r.Success)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Error: {r.ErrorMessage}");
            Console.ResetColor();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        switch (r.Operation)
        {
            case "Compare":
                Console.WriteLine($"Result: {r.Operand1} == {r.Operand2}  →  {r.BoolResult}");
                break;
            case "Convert":
                Console.WriteLine($"Result: {r.Operand1}  →  {r.Result}");
                break;
            case "Divide":
                Console.WriteLine($"Result: {r.ScalarResult:F4}");
                break;
            default:
                Console.WriteLine($"Result: {r.Result?.Value} {r.Result?.Unit}");
                break;
        }
        Console.ResetColor();
    }
}