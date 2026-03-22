using QuantityMeasurementModel;
using QuantityMeasurementModel.DTOs;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementRepository.Data;

namespace QuantityMeasurementRepository.Services
{
    /// <summary>
    /// UC17: EF Core implementation of IQuantityMeasurementRepository.
    /// Every Swagger API call goes through this — saving to SQL Server automatically.
    /// No SqlCommand, no stored procedures, no manual ResultSet mapping.
    /// </summary>
    public class EfQuantityMeasurementRepository : IQuantityMeasurementRepository
    {
        private readonly AppDbContext _db;

        public EfQuantityMeasurementRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            _db.Measurements.Add(ToRecord(entity));
            _db.SaveChanges();
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

        // -- Mapping ----------------------------------------------------------

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