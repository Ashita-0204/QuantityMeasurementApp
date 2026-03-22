using QuantityMeasurementModel;

namespace QuantityMeasurementRepository
{
    /// <summary>
    /// Implementation for Database memory
    /// </summary>
    public sealed class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static readonly Lazy<QuantityMeasurementCacheRepository> _instance = new(() => new QuantityMeasurementCacheRepository());
        //lazy-- singleton concept only one object will exist for he same
        public static QuantityMeasurementCacheRepository Instance => _instance.Value;

        private readonly List<QuantityMeasurementEntity> _cache = new();
        private readonly object _lock = new(); //locking mechanism

        private QuantityMeasurementCacheRepository() { }

        // -- UC15 methods (unchanged) ------------

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

        // --- UC16 new methods ----------

        public IReadOnlyList<QuantityMeasurementEntity> GetByOperation(string operation)
        {
            ArgumentNullException.ThrowIfNull(operation);
            lock (_lock)
            {
                return _cache
                    .Where(e => string.Equals(e.Operation, operation, StringComparison.OrdinalIgnoreCase))
                    .ToList().AsReadOnly();
            }
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetByCategory(string category)
        {
            ArgumentNullException.ThrowIfNull(category);
            lock (_lock)
            {
                return _cache
                    .Where(e => e.Operand1 != null && string.Equals(e.Operand1.Category, category, StringComparison.OrdinalIgnoreCase))
                    .ToList().AsReadOnly();
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
