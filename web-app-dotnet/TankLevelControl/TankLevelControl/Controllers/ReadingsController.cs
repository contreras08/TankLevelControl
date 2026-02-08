using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using TankLevelControl.Data;
using TankLevelControl.DTOs;
using TankLevelControl.Hubs;
using TankLevelControl.Models;

namespace TankLevelControl.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReadingsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<TankHub> _hub;

        // ✅ UN SOLO CONSTRUCTOR (sin duplicados)
        public ReadingsController(AppDbContext context, IHubContext<TankHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        [HttpPost]
        public async Task<IActionResult> PostReading([FromBody] ReadingRequestDto dto)
        {
            // Seguridad: si viene null por JSON mal formado
            if (dto == null)
                return BadRequest(new { message = "Body inválido o vacío." });

            if (string.IsNullOrWhiteSpace(dto.DeviceId))
                return BadRequest(new { message = "deviceId es obligatorio." });

            // 1) Buscar el tanque por DeviceId
            var tank = await _context.Tanks.FirstOrDefaultAsync(t => t.DeviceId == dto.DeviceId);

            if (tank == null)
                return NotFound(new { message = $"No existe un tanque con DeviceId {dto.DeviceId}" });

            // Seguridad: evitar división por cero
            if (tank.HeightCm <= 0)
                return BadRequest(new { message = $"El tanque {tank.Id} tiene HeightCm inválido (<= 0)." });

            // 2) Cálculos
            decimal levelCm = tank.HeightCm - dto.DistanceCm;

            if (levelCm < 0) levelCm = 0;
            if (levelCm > tank.HeightCm) levelCm = tank.HeightCm;

            decimal levelPercent = (levelCm / tank.HeightCm) * 100m;
            decimal volumeLiters = (levelPercent / 100m) * tank.CapacityLiters;

            // 3) Crear la lectura
            var reading = new SensorReading
            {
                TankId = tank.Id,
                DistanceCm = dto.DistanceCm,
                LevelCm = levelCm,
                LevelPercent = levelPercent,
                VolumeLiters = volumeLiters,
                Timestamp = DateTime.UtcNow
            };

            // 4) Guardar en BD
            _context.SensorReadings.Add(reading);
            await _context.SaveChangesAsync();

            // 5) Notificar por SignalR (tiempo real)
            await _hub.Clients.All.SendAsync("tankUpdated", new
            {
                tankId = tank.Id,
                deviceId = tank.DeviceId,
                levelCm,
                levelPercent,
                volumeLiters,
                timestamp = reading.Timestamp
            });

            // 6) Respuesta
            return Ok(new
            {
                tankId = tank.Id,
                levelCm,
                levelPercent,
                volumeLiters,
                timestamp = reading.Timestamp
            });
        }
    }
}
