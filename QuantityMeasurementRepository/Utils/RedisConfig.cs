namespace QuantityMeasurementRepository.Redis
{
    /// <summary>
    /// Holds Redis connection settings.
    /// Reads from environment variable REDIS_URL if set,
    /// otherwise defaults to localhost:6379.
    /// </summary>
    public class RedisConfig
    {
        public string ConnectionString { get; }

        public RedisConfig(string? connectionString = null)
        {
            ConnectionString =
                connectionString
                ?? Environment.GetEnvironmentVariable("REDIS_URL")
                ?? "localhost:6379";
        }
    }
}