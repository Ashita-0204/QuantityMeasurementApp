namespace QuantityMeasurementRepository.Exceptions
{
    /// <summary>
    /// UC16: Wraps any exception that occurs during database operations,
    /// providing a meaningful error message to the upper layers of the application.
    /// Analogous to a custom JDBC exception in the Java reference.
    /// </summary>
    public class DatabaseException : Exception
    {
        public DatabaseException(string message)
            : base(message) { }

        public DatabaseException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
