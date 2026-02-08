using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using TankLevelControl.Models;

namespace TankLevelControl.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Tank> Tanks => Set<Tank>();
        public DbSet<SensorReading> SensorReadings => Set<SensorReading>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tank: DeviceId debe ser único
            modelBuilder.Entity<Tank>()
                .HasIndex(t => t.DeviceId)
                .IsUnique();

            // Relación 1 -> muchos
            modelBuilder.Entity<SensorReading>()
                .HasOne(r => r.Tank)
                .WithMany(t => t.Readings)
                .HasForeignKey(r => r.TankId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed inicial: 3 tanques
            modelBuilder.Entity<Tank>().HasData(
                new Tank { Id = 1, Name = "Tanque 1", CapacityLiters = 10000, HeightCm = 270, DeviceId = "ESP32-T1" },
                new Tank { Id = 2, Name = "Tanque 2", CapacityLiters = 10000, HeightCm = 270, DeviceId = "ESP32-T2" },
                new Tank { Id = 3, Name = "Tanque 3", CapacityLiters = 10000, HeightCm = 270, DeviceId = "ESP32-T3" }
            );
        }
    }
}
