using QuantityMeasurementApp.Models;
using QuantityMeasurementBusinessLayer;
using QuantityMeasurementModel.DTOs;
using QuantityMeasurementRepository;
using QuantityMeasurementRepository.Cache;
using QuantityMeasurementRepository.Database;
using QuantityMeasurementRepository.Util;

#nullable disable

namespace QuantityMeasurementApp
{
    class Program
    {
        public static void Main()
        {
            IQuantityMeasurementRepository repository = AskAndBuildRepository();

            Console.WriteLine($"[App] Using repository: {repository.GetType().Name}");
            Console.WriteLine($"[App] {repository.GetPoolStatistics()}");

            IMenu menu = new Menu();
            menu.ShowMenu(repository);

            Console.WriteLine($"\n[App] Total measurements stored: {repository.GetTotalCount()}");
            Console.WriteLine($"[App] {repository.GetPoolStatistics()}");

            ReportSummary(repository);
            repository.ReleaseResources();
        }

        private static IQuantityMeasurementRepository AskAndBuildRepository()
        {
            Console.WriteLine("=== Select Storage Type ===");
            Console.WriteLine("1 -> Cache (saves to JSON file)");
            Console.WriteLine("2 -> Database (SQL Server)");
            Console.Write("Enter choice: ");

            string input = Console.ReadLine()!.Trim();

            if (input == "2")
            {
                Console.WriteLine("[App] Initialising database repository...");

                DatabaseConfig config;
                QuantityMeasurementDatabaseRepository dbRepository;

                try
                {
                    config = new DatabaseConfig();
                    dbRepository = new QuantityMeasurementDatabaseRepository(config);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[App] ERROR: Could not connect to database: {ex.Message}");
                    Console.WriteLine("[App] Falling back to JSON cache.");
                    return new QuantityMeasurementJsonCacheRepository();
                }

                MigrateJsonCacheToDatabase(dbRepository);
                return dbRepository;
            }

            Console.WriteLine("[App] Initialising JSON cache repository...");
            return new QuantityMeasurementJsonCacheRepository();
        }

        private static void MigrateJsonCacheToDatabase(QuantityMeasurementDatabaseRepository dbRepository)
        {
            const string cacheFile = "measurements.json";
            const string migratedFile = "measurements.json.migrated";

            // ── Restore from .migrated if a previous migration was interrupted ─
            if (!File.Exists(cacheFile) && File.Exists(migratedFile))
            {
                Console.WriteLine("[Migration] No active cache found. Previously migrated file exists at measurements.json.migrated.");
                return;
            }

            if (!File.Exists(cacheFile))
                return;

            Console.WriteLine("[Migration] Found measurements.json — loading records...");

            var jsonRepo = new QuantityMeasurementJsonCacheRepository(cacheFile);
            var pending = jsonRepo.GetAll();

            if (pending.Count == 0)
            {
                Console.WriteLine("[Migration] Cache is empty, nothing to migrate.");
                return;
            }

            Console.WriteLine($"[Migration] {pending.Count} record(s) to migrate. Starting...");
            Console.WriteLine(new string('-', 50));

            // Load IDs already in DB to skip duplicates instead of crashing on PK violation
            var existingIds = new HashSet<Guid>(dbRepository.GetAll().Select(e => e.Id));

            int success = 0;
            int skipped = 0;
            int failed = 0;

            foreach (var entity in pending)
            {
                if (existingIds.Contains(entity.Id))
                {
                    skipped++;
                    Console.WriteLine($"[Migration] ~ SKIP (already in DB): {entity.Operation} — {entity.Id}");
                    continue;
                }

                try
                {
                    dbRepository.Save(entity);
                    success++;
                    Console.WriteLine($"[Migration] ✓ [{success}/{pending.Count}] {entity.Operation} — {entity.Id}");
                }
                catch (Exception ex)
                {
                    failed++;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[Migration] ✗ FAILED: {entity.Id} — {ex.Message}");
                    if (ex.InnerException != null)
                        Console.WriteLine($"            Inner: {ex.InnerException.Message}");
                    Console.ResetColor();
                }
            }

            Console.WriteLine(new string('-', 50));
            Console.WriteLine($"[Migration] Result: {success} migrated, {skipped} skipped (already in DB), {failed} failed.");

            if (failed > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[Migration] WARNING: {failed} record(s) failed. measurements.json NOT renamed.");
                Console.ResetColor();
                return;
            }

            // All succeeded — rename so it won't be re-imported
            File.Move(cacheFile, migratedFile, overwrite: true);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[Migration] All records migrated successfully.");
            Console.WriteLine($"[Migration] Cache renamed to '{migratedFile}' — it won't be re-imported.");
            Console.ResetColor();
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