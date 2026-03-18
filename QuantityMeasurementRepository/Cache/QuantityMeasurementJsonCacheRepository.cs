using System.Text.Json;
using QuantityMeasurementModel;
using QuantityMeasurementModel.DTOs;

namespace QuantityMeasurementRepository.Cache
{
    /// <summary>
    /// UC16: Cache-based repository that also persists every operation to a JSON file.
    /// On startup it loads existing records from the file so history is not lost between runs.
    /// </summary>
    public class QuantityMeasurementJsonCacheRepository : IQuantityMeasurementRepository
    {
        private readonly string _filePath;
        private readonly List<QuantityMeasurementEntity> _cache = new();
        private readonly object _lock = new();

        private static readonly JsonSerializerOptions _jsonOptions =
            new() { WriteIndented = true };

        public QuantityMeasurementJsonCacheRepository(string filePath = "measurements.json")
        {
            _filePath = filePath;
            LoadFromFile();
        }

        // ── UC15 methods ──────────────────────────────────────────────────────

        public void Save(QuantityMeasurementEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            lock (_lock)
            {
                _cache.Add(entity);
                WriteToFile();
            }
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetAll()
        {
            lock (_lock) { return _cache.AsReadOnly(); }
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetRecent(int count)
        {
            lock (_lock)
            {
                return _cache
                    .OrderByDescending(e => e.Timestamp)
                    .Take(count)
                    .ToList()
                    .AsReadOnly();
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _cache.Clear();
                WriteToFile();
            }
        }

        // ── UC16 methods ──────────────────────────────────────────────────────

        public IReadOnlyList<QuantityMeasurementEntity> GetByOperation(string operation)
        {
            ArgumentNullException.ThrowIfNull(operation);
            lock (_lock)
            {
                return _cache
                    .Where(e => string.Equals(e.Operation, operation, StringComparison.OrdinalIgnoreCase))
                    .ToList()
                    .AsReadOnly();
            }
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetByCategory(string category)
        {
            ArgumentNullException.ThrowIfNull(category);
            lock (_lock)
            {
                return _cache
                    .Where(e => e.Operand1 != null &&
                                string.Equals(e.Operand1.Category, category, StringComparison.OrdinalIgnoreCase))
                    .ToList()
                    .AsReadOnly();
            }
        }

        public int GetTotalCount()
        {
            lock (_lock) { return _cache.Count; }
        }

        public void DeleteAll()
        {
            Clear();
        }

        public string GetPoolStatistics() =>
            $"[JsonCacheRepository] File: {_filePath} | Entries in cache: {_cache.Count}";

        // ── Private helpers ───────────────────────────────────────────────────

        // Reads existing records from the JSON file into memory when the app starts.
        private void LoadFromFile()
        {
            if (!File.Exists(_filePath)) return;

            string json = File.ReadAllText(_filePath);
            List<EntityRow>? rows = JsonSerializer.Deserialize<List<EntityRow>>(json);
            if (rows == null) return;

            foreach (EntityRow row in rows)
            {
                QuantityMeasurementEntity entity = QuantityMeasurementEntity.Reconstruct(
                    row.Id, row.Timestamp, row.Operation,
                    row.Operand1, row.Operand2, row.Result,
                    row.BoolResult, row.ScalarResult,
                    row.HasError, row.ErrorMessage ?? string.Empty);

                _cache.Add(entity);
            }

            Console.WriteLine($"[JsonCacheRepository] Loaded {_cache.Count} existing record(s) from {_filePath}");
        }

        // Writes the full cache to the JSON file after every change.
        private void WriteToFile()
        {
            List<EntityRow> rows = _cache.Select(e => new EntityRow
            {
                Id = e.Id,
                Timestamp = e.Timestamp,
                Operation = e.Operation,
                Operand1 = e.Operand1,
                Operand2 = e.Operand2,
                Result = e.Result,
                BoolResult = e.BoolResult,
                ScalarResult = e.ScalarResult,
                HasError = e.HasError,
                ErrorMessage = e.ErrorMessage
            }).ToList();

            string json = JsonSerializer.Serialize(rows, _jsonOptions);
            File.WriteAllText(_filePath, json);
        }

        // ── Serialization helper (mirrors QuantityMeasurementEntity fields) ──

        private class EntityRow
        {
            public Guid Id { get; set; }
            public DateTime Timestamp { get; set; }
            public string Operation { get; set; } = string.Empty;
            public QuantityDTO? Operand1 { get; set; }
            public QuantityDTO? Operand2 { get; set; }
            public QuantityDTO? Result { get; set; }
            public bool? BoolResult { get; set; }
            public double? ScalarResult { get; set; }
            public bool HasError { get; set; }
            public string? ErrorMessage { get; set; }
        }
    }
}