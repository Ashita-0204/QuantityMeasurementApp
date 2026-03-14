using QuantityMeasurementModel;

namespace QuantityMeasurementRepository
{
    /// <summary>
    /// UC16 update: implements the three new interface methods added in UC16.
    /// All UC15 logic is untouched.
    /// </summary>
    public sealed class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static readonly Lazy<QuantityMeasurementCacheRepository> _instance =
            new(() => new QuantityMeasurementCacheRepository());

        public static QuantityMeasurementCacheRepository Instance => _instance.Value;

        private readonly List<QuantityMeasurementEntity> _cache = new();
        private readonly object _lock = new();

        private QuantityMeasurementCacheRepository() { }

        // ── UC15 methods (unchanged) ──────────────────────────────────────────

        public void Save(QuantityMeasurementEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            lock (_lock) { _cache.Add(entity); }
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetAll()
        {
            lock (_lock) { return _cache.AsReadOnly(); }
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetRecent(int count)
        {
            lock (_lock)
            {
                return _cache.OrderByDescending(e => e.Timestamp).Take(count).ToList().AsReadOnly();
            }
        }

        public void Clear()
        {
            lock (_lock) { _cache.Clear(); }
        }

        // ── UC16 new methods ──────────────────────────────────────────────────

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
            Clear(); // same effect for in-memory store
        }
    }
}
