using QuantityMeasurementApp.Models;
using QuantityMeasurementBusinessLayer;
using QuantityMeasurementModel.DTOs;
using QuantityMeasurementRepository;
using QuantityMeasurementRepository.Database;
using QuantityMeasurementRepository.Util;

#nullable disable

namespace QuantityMeasurementApp
{
    /// <summary>
    /// UC16: Program entry point.
    /// Reads appsettings.json via DatabaseConfig to decide whether to use
    /// the in-memory CacheRepository (UC15) or the new SQLite DatabaseRepository (UC16).
    /// All business logic in QuantityMeasurementServiceImpl is untouched.
    /// </summary>
    class Program
    {
        public static void Main()
        {
            // ── 1. Load config and pick repository ────────────────────────────
            DatabaseConfig config = new();
            IQuantityMeasurementRepository repository = BuildRepository(config);

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

        private static IQuantityMeasurementRepository BuildRepository(DatabaseConfig config)
        {
            if (string.Equals(config.RepositoryType, "database", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("[App] Initialising database repository (SQLite / ADO.NET)...");
                return new QuantityMeasurementDatabaseRepository(config);
            }

            Console.WriteLine("[App] Initialising in-memory cache repository...");
            return QuantityMeasurementCacheRepository.Instance;
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
