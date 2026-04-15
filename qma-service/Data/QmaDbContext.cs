using Microsoft.EntityFrameworkCore;
using QmaService.Entities;

namespace QmaService.Data
{
    public class QmaDbContext : DbContext
    {
        public QmaDbContext(DbContextOptions<QmaDbContext> options) : base(options) { }

        public DbSet<MeasurementRecord> Measurements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MeasurementRecord>(entity =>
            {
                entity.ToTable("measurements");
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Id).HasColumnName("id");
                entity.Property(m => m.UserId).HasColumnName("user_id");
                entity.Property(m => m.Operation).HasColumnName("operation").IsRequired();
                entity.Property(m => m.Timestamp).HasColumnName("timestamp");
                entity.Property(m => m.Operand1Value).HasColumnName("operand1_value");
                entity.Property(m => m.Operand1Unit).HasColumnName("operand1_unit");
                entity.Property(m => m.Operand1Category).HasColumnName("operand1_category");
                entity.Property(m => m.Operand2Value).HasColumnName("operand2_value");
                entity.Property(m => m.Operand2Unit).HasColumnName("operand2_unit");
                entity.Property(m => m.ResultValue).HasColumnName("result_value");
                entity.Property(m => m.ResultUnit).HasColumnName("result_unit");
                entity.Property(m => m.ResultCategory).HasColumnName("result_category");
                entity.Property(m => m.BoolResult).HasColumnName("bool_result");
                entity.Property(m => m.ScalarResult).HasColumnName("scalar_result");
                entity.HasIndex(m => m.UserId);
                entity.HasIndex(m => m.Timestamp);
            });
        }
    }
}
