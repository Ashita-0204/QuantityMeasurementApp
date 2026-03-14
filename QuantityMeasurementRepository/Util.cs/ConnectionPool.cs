using Microsoft.Data.SqlClient;

namespace QuantityMeasurementRepository.Util
{
    // This class gives us a database connection whenever we need one.
    // ADO.NET already handles connection pooling internally for SQL Server,
    // so we just need to open and close connections properly.
    public class ConnectionPool
    {
        private readonly string _connectionString;

        public ConnectionPool(DatabaseConfig config)
        {
            _connectionString = config.ConnectionString;
        }

        // Call this to get an open connection to the database
        public SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        // Returns a basic stats message
        public string GetStatistics()
        {
            return $"[ConnectionPool] Using SQL Server: {_connectionString.Split(';')[0]}";
        }
    }
}