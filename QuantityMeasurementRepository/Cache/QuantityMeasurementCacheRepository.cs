using QuantityMeasurementModel;

namespace QuantityMeasurementRepository
{
    public sealed class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static readonly Lazy<QuantityMeasurementCacheRepository> _instance =
            new(() => new QuantityMeasurementCacheRepository());

        public static QuantityMeasurementCacheRepository Instance => _instance.Value;

        private readonly List<QuantityMeasurementEntity> _cache = new();
        private readonly object _lock = new();

        private QuantityMeasurementCacheRepository() { }

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
    }
}