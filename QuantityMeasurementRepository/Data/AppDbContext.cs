using Microsoft.EntityFrameworkCore;
using QuantityMeasurementModel.Entities;

namespace QuantityMeasurementRepository.Data
{
    /// <summary>
    /// UC17: EF Core DbContext. Lives in Repository layer.
    /// Replaces manual ADO.NET from UC16.
    /// Migrations: dotnet ef migrations add InitialCreate --project QuantityMeasurementApi
    ///             dotnet ef database update --project QuantityMeasurementApi
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<MeasurementRecord> Measurements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>(e =>
            {
                e.ToTable("users");
                e.HasIndex(u => u.Username).IsUnique();
                e.HasIndex(u => u.Email).IsUnique();
            });

            modelBuilder.Entity<MeasurementRecord>(e =>
            {
                e.ToTable("quantity_measurements_ef");
                e.HasIndex(m => m.Operation);
                e.HasIndex(m => m.Operand1Category);
                e.HasIndex(m => m.Timestamp);
            });
        }
    }
}