using System.Text.Json;

namespace QuantityMeasurementRepository.Util
{
    public class DatabaseConfig
    {
        public virtual string ConnectionString { get; } = string.Empty;
        public virtual string RepositoryType { get; } = string.Empty;

        // Used by the real app - reads appsettings.json
        public DatabaseConfig()
        {
            var path = FindAppSettings();
            var doc = JsonDocument.Parse(File.ReadAllText(path));

            ConnectionString = doc.RootElement
                .GetProperty("db")
                .GetProperty("connectionString")
                .GetString()!;

            RepositoryType = doc.RootElement
                .GetProperty("app")
                .GetProperty("repositoryType")
                .GetString()!;
        }

        // Used by test subclasses that override the properties directly
        // Does NOT read any file
        public DatabaseConfig(string? ignored) { }

        private static string FindAppSettings()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            for (int i = 0; i < 8; i++)
            {
                if (dir == null) break;
                var candidate = Path.Combine(dir.FullName, "appsettings.json");
                if (File.Exists(candidate)) return candidate;
                dir = dir.Parent;
            }
            throw new FileNotFoundException("appsettings.json not found.");
        }
    }
}