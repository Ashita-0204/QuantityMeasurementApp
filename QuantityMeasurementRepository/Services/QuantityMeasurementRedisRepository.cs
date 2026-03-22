using System.Text.Json;
using QuantityMeasurementModel;
using QuantityMeasurementModel.DTOs;
using QuantityMeasurementRepository.Redis;
using StackExchange.Redis;

namespace QuantityMeasurementRepository.Services
{
    /// <summary>
    /// UC17 (Console): Redis implementation of IQuantityMeasurementRepository.
    ///
    /// Key design:
    ///   measurement:{id}          → JSON string of the full entity
    ///   measurements:all          → Redis List of all IDs (insertion order)
    ///   measurements:op:{op}      → Redis Set of IDs by operation  (e.g. measurements:op:Add)
    ///   measurements:cat:{cat}    → Redis Set of IDs by category   (e.g. measurements:cat:Length)
    ///   measurements:ts           → Redis Sorted Set  id → timestamp score (for GetRecent)
    ///
    /// This replaces the ADO.NET QuantityMeasurementDatabaseRepository from UC16.
    /// No stored procedures, no SqlConnection, no manual ResultSet mapping.
    /// </summary>
    public class QuantityMeasurementRedisRepository : IQuantityMeasurementRepository
    {
        private readonly IDatabase _db;
        private readonly ConnectionMultiplexer _redis;

        private static readonly JsonSerializerOptions _json =
            new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        // Redis key constants
        private const string AllKey = "measurements:all";
        private const string TsKey = "measurements:ts";

        public QuantityMeasurementRedisRepository(RedisConfig? config = null)
        {
            config ??= new RedisConfig();
            _redis = ConnectionMultiplexer.Connect(config.ConnectionString);
            _db = _redis.GetDatabase();
            Console.WriteLine($"[RedisRepository] Connected to Redis at {config.ConnectionString}");
        }

        // ── Save ─────────────────────────────────────────────────────────────
        public void Save(QuantityMeasurementEntity entity)
        {
            string id = entity.Id.ToString();
            string key = EntityKey(id);

            // Skip if already exists (prevents duplicate key issues on re-run)
            if (_db.KeyExists(key))
                return;

            string json = JsonSerializer.Serialize(ToRow(entity), _json);

            // Store the entity JSON
            _db.StringSet(key, json);

            // Add to all-IDs list
            _db.ListRightPush(AllKey, id);

            // Index by operation
            if (!string.IsNullOrEmpty(entity.Operation))
                _db.SetAdd(OpKey(entity.Operation), id);

            // Index by category
            if (entity.Operand1?.Category != null)
                _db.SetAdd(CatKey(entity.Operand1.Category), id);

            // Add to sorted set with timestamp score for GetRecent
            _db.SortedSetAdd(TsKey, id, ToScore(entity.Timestamp));
        }

        // ── GetAll ───────────────────────────────────────────────────────────
        public IReadOnlyList<QuantityMeasurementEntity> GetAll()
        {
            var ids = _db.ListRange(AllKey);
            return FetchByIds(ids.Select(x => x.ToString()));
        }

        // ── GetRecent ────────────────────────────────────────────────────────
        public IReadOnlyList<QuantityMeasurementEntity> GetRecent(int count)
        {
            // Sorted set is ordered by timestamp score ascending; take from the end
            var ids = _db.SortedSetRangeByRank(TsKey, -count, -1, Order.Descending);
            return FetchByIds(ids.Select(x => x.ToString()));
        }

        // ── GetByOperation ───────────────────────────────────────────────────
        public IReadOnlyList<QuantityMeasurementEntity> GetByOperation(string operation)
        {
            var ids = _db.SetMembers(OpKey(operation));
            return FetchByIds(ids.Select(x => x.ToString()));
        }

        // ── GetByCategory ────────────────────────────────────────────────────
        public IReadOnlyList<QuantityMeasurementEntity> GetByCategory(string category)
        {
            var ids = _db.SetMembers(CatKey(category));
            return FetchByIds(ids.Select(x => x.ToString()));
        }

        // ── GetTotalCount ─────────────────────────────────────────────────────
        public int GetTotalCount() => (int)_db.ListLength(AllKey);

        // ── Clear / DeleteAll ────────────────────────────────────────────────
        public void Clear() => DeleteAll();

        public void DeleteAll()
        {
            // Delete every individual entity key
            var ids = _db.ListRange(AllKey).Select(x => x.ToString()).ToList();
            foreach (var id in ids)
                _db.KeyDelete(EntityKey(id));

            // Delete index keys
            _db.KeyDelete(AllKey);
            _db.KeyDelete(TsKey);

            // Delete op and cat index sets
            foreach (var op in new[] { "Compare", "Convert", "Add", "Subtract", "Divide" })
                _db.KeyDelete(OpKey(op));

            foreach (var cat in new[] { "Length", "Weight", "Volume", "Temperature" })
                _db.KeyDelete(CatKey(cat));

            Console.WriteLine("[RedisRepository] All measurements deleted.");
        }

        public string GetPoolStatistics()
        {
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            return $"[RedisRepository] Redis {server.Version} | Total records: {GetTotalCount()}";
        }

        public void ReleaseResources()
        {
            _redis.Dispose();
            Console.WriteLine("[RedisRepository] Connection closed.");
        }

        // ── Private helpers ──────────────────────────────────────────────────

        private IReadOnlyList<QuantityMeasurementEntity> FetchByIds(IEnumerable<string> ids)
        {
            var result = new List<QuantityMeasurementEntity>();

            foreach (var id in ids)
            {
                var val = _db.StringGet(EntityKey(id));
                if (val.IsNullOrEmpty) continue;

                var row = JsonSerializer.Deserialize<EntityRow>((string)val!, _json);
                if (row != null)
                    result.Add(ToEntity(row));
            }

            return result.AsReadOnly();
        }

        private static string EntityKey(string id) => $"measurement:{id}";
        private static string OpKey(string op) => $"measurements:op:{op.ToLower()}";
        private static string CatKey(string cat) => $"measurements:cat:{cat.ToLower()}";

        // Convert DateTime to a double score (Unix timestamp in ms)
        private static double ToScore(DateTime dt) =>
            (dt.ToUniversalTime() - DateTime.UnixEpoch).TotalMilliseconds;

        // ── Mapping ──────────────────────────────────────────────────────────

        private static EntityRow ToRow(QuantityMeasurementEntity e) => new()
        {
            Id = e.Id,
            Timestamp = e.Timestamp,
            Operation = e.Operation,
            Operand1Value = e.Operand1?.Value,
            Operand1Unit = e.Operand1?.Unit,
            Operand1Category = e.Operand1?.Category,
            Operand2Value = e.Operand2?.Value,
            Operand2Unit = e.Operand2?.Unit,
            Operand2Category = e.Operand2?.Category,
            ResultValue = e.Result?.Value,
            ResultUnit = e.Result?.Unit,
            ResultCategory = e.Result?.Category,
            BoolResult = e.BoolResult,
            ScalarResult = e.ScalarResult,
            HasError = e.HasError,
            ErrorMessage = e.ErrorMessage
        };

        private static QuantityMeasurementEntity ToEntity(EntityRow r)
        {
            QuantityDTO? op1 = r.Operand1Value.HasValue
                ? new QuantityDTO(r.Operand1Value.Value, r.Operand1Unit!, r.Operand1Category!)
                : null;

            QuantityDTO? op2 = r.Operand2Value.HasValue
                ? new QuantityDTO(r.Operand2Value.Value, r.Operand2Unit!, r.Operand2Category!)
                : null;

            QuantityDTO? res = r.ResultValue.HasValue
                ? new QuantityDTO(r.ResultValue.Value, r.ResultUnit!, r.ResultCategory!)
                : null;

            return QuantityMeasurementEntity.Reconstruct(
                r.Id, r.Timestamp, r.Operation,
                op1, op2, res,
                r.BoolResult, r.ScalarResult,
                r.HasError, r.ErrorMessage ?? string.Empty);
        }

    }
}