using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TankLevelControl.Models
{
    public class Tank
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Capacidad nominal del tanque en litros (ej: 10000).
        /// </summary>
        [Range(1, int.MaxValue)]
        public int CapacityLiters { get; set; } = 10000;

        /// <summary>
        /// Altura interna aproximada del tanque en cm (ej: 270).
        /// </summary>
        [Range(1, 100000)]
        public int HeightCm { get; set; } = 270;

        /// <summary>
        /// Identificador único del dispositivo/ESP32 (ej: ESP32-T1).
        /// </summary>
        [Required, MaxLength(50)]
        public string DeviceId { get; set; } = string.Empty;

        // Navegación (1 tanque -> muchas lecturas)
        public List<SensorReading> Readings { get; set; } = new();
    }
}
