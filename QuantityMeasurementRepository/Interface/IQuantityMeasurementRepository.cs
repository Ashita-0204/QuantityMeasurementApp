using QuantityMeasurementModel;

namespace QuantityMeasurementRepository
{
    public interface IQuantityMeasurementRepository
    {
        // -- UC15 methods (unchanged) ----------
        void Save(QuantityMeasurementEntity entity);
        IReadOnlyList<QuantityMeasurementEntity> GetAll();
        IReadOnlyList<QuantityMeasurementEntity> GetRecent(int count);
        void Clear();

        // ---- UC16 new query methods -----
        //Returns all measurements whose Operation matches the given string (e.g. "Compare")
        IReadOnlyList<QuantityMeasurementEntity> GetByOperation(string operation);

        //Returns all measurements whose Operand1 category matches (e.g. "Length")
        IReadOnlyList<QuantityMeasurementEntity> GetByCategory(string category);

        //Returns the total number of stored measurements.
        int GetTotalCount();

        //Permanently deletes every measurement from the store.</summary>
        void DeleteAll();
        string GetPoolStatistics() => "Pool statistics not available for this repository type.";
        //releases resources in the pool
        void ReleaseResources() { }
    }
}
