using QuantityMeasurementModel;

namespace QuantityMeasurementRepository
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);
        IReadOnlyList<QuantityMeasurementEntity> GetAll();
        IReadOnlyList<QuantityMeasurementEntity> GetRecent(int count);
        void Clear();
    }
}