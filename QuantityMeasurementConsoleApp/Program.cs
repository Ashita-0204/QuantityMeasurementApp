using QuantityMeasurementBusinessLayer;
using QuantityMeasurementRepository;
using QuantityMeasurementRepository.Cache;
using QuantityMeasurementRepository.Redis;
using QuantityMeasurementRepository.Services;

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
            ReportSummary(repository);
            repository.ReleaseResources();
        }

        private static IQuantityMeasurementRepository AskAndBuildRepository()
        {
            Console.WriteLine("=== Select Storage Type ===");
            Console.WriteLine("1 -> Cache (saves to JSON file)");
            Console.WriteLine("2 -> Redis");
            Console.Write("Enter choice: ");

            string input = Console.ReadLine()!.Trim();

            if (input == "2")
            {
                try
                {
                    var config = new RedisConfig();
                    var redisRepo = new QuantityMeasurementRedisRepository(config);

                    // Migrate any pending JSON cache into Redis
                    MigrateJsonCacheToRedis(redisRepo);

                    return redisRepo;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[App] Could not connect to Redis: {ex.Message}");
                    Console.WriteLine("[App] Falling back to JSON cache.");
                    Console.ResetColor();
                    return new QuantityMeasurementJsonCacheRepository();
                }
            }

            Console.WriteLine("[App] Initialising JSON cache repository...");
            return new QuantityMeasurementJsonCacheRepository();
        }

        private static void MigrateJsonCacheToRedis(QuantityMeasurementRedisRepository redisRepo)
        {
            const string cacheFile = "measurements.json";
            const string migratedFile = "measurements.json.migrated";

            if (!File.Exists(cacheFile)) return;

            Console.WriteLine("[Migration] Found measurements.json — migrating to Redis...");

            var jsonRepo = new QuantityMeasurementJsonCacheRepository(cacheFile);
            var pending = jsonRepo.GetAll();

            if (pending.Count == 0)
            {
                Console.WriteLine("[Migration] Cache is empty, nothing to migrate.");
                return;
            }

            Console.WriteLine($"[Migration] {pending.Count} record(s) to migrate...");

            int success = 0;
            foreach (var entity in pending)
            {
                // Redis Save() already skips duplicates via KeyExists check
                redisRepo.Save(entity);
                success++;
                Console.WriteLine($"[Migration] ✓ [{success}/{pending.Count}] {entity.Operation}");
            }

            Console.WriteLine($"[Migration] Done — {success} migrated.");
            File.Move(cacheFile, migratedFile, overwrite: true);
            Console.WriteLine($"[Migration] Cache renamed to '{migratedFile}'.");
        }

        private static void ReportSummary(IQuantityMeasurementRepository repository)
        {
            Console.WriteLine("\n-- Measurement Summary ------------------------------");

            foreach (string op in new[] { "Compare", "Convert", "Add", "Subtract", "Divide" })
            {
                int count = repository.GetByOperation(op).Count;
                if (count > 0) Console.WriteLine($"  {op,-12}: {count} record(s)");
            }

            Console.WriteLine("-- By Category ---------------------------------------");
            foreach (string cat in new[] { "Length", "Weight", "Volume", "Temperature" })
            {
                int count = repository.GetByCategory(cat).Count;
                if (count > 0) Console.WriteLine($"  {cat,-12}: {count} record(s)");
            }

            Console.WriteLine($"-- Total: {repository.GetTotalCount()} -----------------------------\n");
        }
    }
}