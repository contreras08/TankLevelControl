namespace TankLevelControl.DTOs
{
    public class ReadingRequestDto
    {
        public string DeviceId { get; set; } = string.Empty;

        // Distancia medida por el sensor en centímetros
        public decimal DistanceCm { get; set; }
    }
}
