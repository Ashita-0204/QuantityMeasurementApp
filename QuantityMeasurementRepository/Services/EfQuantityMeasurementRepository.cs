using QuantityMeasurementModel;
using QuantityMeasurementModel.DTOs;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementRepository.Data;

namespace QuantityMeasurementRepository.Services
{
    /// <summary>
    /// EF Core implementation of IQuantityMeasurementRepository.
    ///
    /// IMPORTANT: Save() is intentionally a no-op here.
    /// The service layer calls _repository.Save() after every compute operation,
    /// but in the web API those records are saved PER USER via the explicit
    /// /api/QuantityMeasurement/save endpoint (which carries the JWT userId).
    /// Letting Save() insert here would produce UserId=0 rows (no user context
    /// available in the repository layer) and cause a NOT NULL / FK violation.
    /// </summary>
    public class EfQuantityMeasurementRepository : IQuantityMeasurementRepository
    {
        private readonly AppDbContext _db;

        public EfQuantityMeasurementRepository(AppDbContext db)
        {
            _db = db;
        }

        // ── No-op: user-scoped saving is handled by the controller ──────────
        public void Save(QuantityMeasurementEntity entity)
        {
            // Intentionally empty.
            // The QuantityMeasurementServiceImpl calls this after every compute,
            // but the EF repository has no UserId here. History is persisted by
            // the /save and /save-batch controller endpoints which have the JWT claim.
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetAll()
            => _db.Measurements.OrderByDescending(m => m.Timestamp)
                .AsEnumerable().Select(ToEntity).ToList().AsReadOnly();

        public IReadOnlyList<QuantityMeasurementEntity> GetRecent(int count)
            => _db.Measurements.OrderByDescending(m => m.Timestamp).Take(count)
                .AsEnumerable().Select(ToEntity).ToList().AsReadOnly();

        public IReadOnlyList<QuantityMeasurementEntity> GetByOperation(string operation)
            => _db.Measurements
                .Where(m => m.Operation.ToLower() == operation.ToLower())
                .OrderByDescending(m => m.Timestamp)
                .AsEnumerable().Select(ToEntity).ToList().AsReadOnly();

        public IReadOnlyList<QuantityMeasurementEntity> GetByCategory(string category)
            => _db.Measurements
                .Where(m => m.Operand1Category != null &&
                            m.Operand1Category.ToLower() == category.ToLower())
                .OrderByDescending(m => m.Timestamp)
                .AsEnumerable().Select(ToEntity).ToList().AsReadOnly();

        public int GetTotalCount() => _db.Measurements.Count();

        public void Clear() => DeleteAll();

        public void DeleteAll()
        {
            _db.Measurements.RemoveRange(_db.Measurements);
            _db.SaveChanges();
        }

        public string GetPoolStatistics()
            => $"[EfRepository] SQL Server via EF Core | Records: {_db.Measurements.Count()}";

        // ── Mapping ─────────────────────────────────────────────────────────

        private static MeasurementRecord ToRecord(QuantityMeasurementEntity e) => new()
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

        private static QuantityMeasurementEntity ToEntity(MeasurementRecord r)
        {
            QuantityDTO? op1 = r.Operand1Value.HasValue
                ? new QuantityDTO(r.Operand1Value.Value, r.Operand1Unit!, r.Operand1Category!) : null;
            QuantityDTO? op2 = r.Operand2Value.HasValue
                ? new QuantityDTO(r.Operand2Value.Value, r.Operand2Unit!, r.Operand2Category!) : null;
            QuantityDTO? res = r.ResultValue.HasValue
                ? new QuantityDTO(r.ResultValue.Value, r.ResultUnit!, r.ResultCategory!) : null;

            return QuantityMeasurementEntity.Reconstruct(
                r.Id, r.Timestamp, r.Operation,
                op1, op2, res,
                r.BoolResult, r.ScalarResult,
                r.HasError, r.ErrorMessage ?? string.Empty);
        }
    }
}