using QuantityMeasurementModel;

namespace QuantityMeasurementRepository
{
    /// <summary>
    /// UC16: Extended repository interface adding DB-oriented query and lifecycle methods.
    /// UC15 methods (Save, GetAll, GetRecent, Clear) are preserved unchanged.
    /// </summary>
    public interface IQuantityMeasurementRepository
    {
        // ── UC15 methods (unchanged) ─────────────────────────────────────────
        void Save(QuantityMeasurementEntity entity);
        IReadOnlyList<QuantityMeasurementEntity> GetAll();
        IReadOnlyList<QuantityMeasurementEntity> GetRecent(int count);
        void Clear();

        // ── UC16 new query methods ────────────────────────────────────────────
        /// <summary>Returns all measurements whose Operation matches the given string (e.g. "Compare").</summary>
        IReadOnlyList<QuantityMeasurementEntity> GetByOperation(string operation);

        /// <summary>Returns all measurements whose Operand1 category matches (e.g. "Length").</summary>
        IReadOnlyList<QuantityMeasurementEntity> GetByCategory(string category);

        /// <summary>Returns the total number of stored measurements.</summary>
        int GetTotalCount();

        /// <summary>Permanently deletes every measurement from the store.</summary>
        void DeleteAll();

        // ── UC16 default interface methods (optional override) ────────────────
        /// <summary>
        /// Returns a human-readable pool/cache statistics string.
        /// Default implementation returns a "not available" message;
        /// database repositories override this with real pool info.
        /// </summary>
        string GetPoolStatistics() => "Pool statistics not available for this repository type.";

        /// <summary>
        /// Releases any resources held by this repository (e.g. DB connections).
        /// Default is a no-op; database repositories override to close the pool.
        /// </summary>
        void ReleaseResources() { }
    }
}
