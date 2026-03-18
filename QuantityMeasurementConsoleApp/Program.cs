// using QuantityMeasurementApp.Model;
using QuantityMeasurementBusinessLayer;
using QuantityMeasurementModel.DTOs;
using QuantityMeasurementRepository;
using QuantityMeasurementRepository.Cache;
using QuantityMeasurementRepository.Database;
using QuantityMeasurementRepository.Util;

#nullable disable

namespace QuantityMeasurementApp
{
    /// <summary>
    /// UC16: Program entry point.
    /// Asks the user at startup whether to use the JSON cache or the SQL Server database.
    /// All business logic in QuantityMeasurementServiceImpl is untouched.
    /// </summary>
    class Program
    {
        public static void Main()
        {
            // ── 1. Ask the user which repository to use ───────────────────────
            IQuantityMeasurementRepository repository = AskAndBuildRepository();

            Console.WriteLine($"[App] Using repository: {repository.GetType().Name}");
            Console.WriteLine($"[App] {repository.GetPoolStatistics()}");

            // ── 2. Run the menu (same as UC15) ────────────────────────────────
            IMenu menu = new Menu();
            menu.ShowMenu(repository);

            // ── 3. After menu exits: report stats and clean up ─────────────────
            Console.WriteLine($"\n[App] Total measurements stored: {repository.GetTotalCount()}");
            Console.WriteLine($"[App] {repository.GetPoolStatistics()}");

            // Demonstrate UC16 query methods before cleanup
            ReportSummary(repository);

            // Release DB connections / dispose pool
            repository.ReleaseResources();
        }

        // ── Private helpers ───────────────────────────────────────────────────

        private static IQuantityMeasurementRepository AskAndBuildRepository()
        {
            Console.WriteLine("=== Select Storage Type ===");
            Console.WriteLine("1 -> Cache (saves to JSON file)");
            Console.WriteLine("2 -> Database (SQL Server)");
            Console.Write("Enter choice: ");

            string input = Console.ReadLine()!.Trim();

            if (input == "2")
            {
                Console.WriteLine("[App] Initialising database repository (SQL Server / ADO.NET)...");
                DatabaseConfig config = new();
                return new QuantityMeasurementDatabaseRepository(config);
            }

            Console.WriteLine("[App] Initialising JSON cache repository...");
            return new QuantityMeasurementJsonCacheRepository();
        }

        private static void ReportSummary(IQuantityMeasurementRepository repository)
        {
            Console.WriteLine("\n── Measurement Summary ──────────────────────────────");

            var operations = new[] { "Compare", "Convert", "Add", "Subtract", "Divide" };
            foreach (string op in operations)
            {
                int count = repository.GetByOperation(op).Count;
                if (count > 0)
                    Console.WriteLine($"  {op,-12}: {count} record(s)");
            }

            var categories = new[] { "Length", "Weight", "Volume", "Temperature" };
            Console.WriteLine("── By Category ───────────────────────────────────────");
            foreach (string cat in categories)
            {
                int count = repository.GetByCategory(cat).Count;
                if (count > 0)
                    Console.WriteLine($"  {cat,-12}: {count} record(s)");
            }

            Console.WriteLine($"── Total: {repository.GetTotalCount()} ──────────────────────────────\n");
        }
    }
}