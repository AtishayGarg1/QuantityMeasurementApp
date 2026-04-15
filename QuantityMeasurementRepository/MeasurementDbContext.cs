using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuantityMeasurementModel.Entities;

namespace QuantityMeasurementRepository
{
    public class MeasurementDbContext : IdentityDbContext<IdentityUser>
    {
        public MeasurementDbContext(DbContextOptions<MeasurementDbContext> options)
            : base(options)
        {
        }

        public DbSet<MeasurementEntity> Measurements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MeasurementEntity>(entity =>
            {
                entity.ToTable("QuantityMeasurementHistory");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.MeasurementCategory).HasMaxLength(50);
                entity.Property(e => e.OperationType).HasMaxLength(50);
                entity.Property(e => e.MeasurementUnit1).HasMaxLength(50);
                entity.Property(e => e.MeasurementUnit2).HasMaxLength(50);
                entity.Property(e => e.TargetMeasurementUnit).HasMaxLength(50);
                entity.Property(e => e.ErrorMessage).HasMaxLength(255);
                entity.Property(e => e.FormattedMessage).HasMaxLength(255);
            });
        }
    }
}
