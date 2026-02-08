using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TankLevelControl.Models
{
    public class SensorReading
    {
        public long Id { get; set; }

        [Required]
        public int TankId { get; set; }

        public Tank? Tank { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Distancia medida por el sensor desde la parte superior hacia el agua (cm).
        /// </summary>
        [Range(0, 100000)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal DistanceCm { get; set; }

        /// <summary>
        /// Nivel calculado (altura de líquido) en cm.
        /// levelCm = heightCm - distanceCm
        /// </summary>
        [Range(0, 100000)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal LevelCm { get; set; }

        /// <summary>
        /// Porcentaje del nivel (0-100).
        /// </summary>
        [Range(0, 100)]
        [Column(TypeName = "decimal(5,2)")]
        public decimal LevelPercent { get; set; }

        /// <summary>
        /// Volumen aproximado en litros (0 - CapacityLiters).
        /// </summary>
        [Range(0, 100000000)]
        [Column(TypeName = "decimal(12,2)")]
        public decimal VolumeLiters { get; set; }
    }
}
