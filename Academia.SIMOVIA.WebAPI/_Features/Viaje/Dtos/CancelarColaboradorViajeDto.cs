namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    public class CancelarColaboradorViajeDto
    {
        public int ViajeEncabezadoId { get; set; }
        public decimal DistanciaTotalKm { get; set; }
        public decimal Total { get; set; }

        public int ViajeDetalleId { get; set; }
        public int ColaboradorId { get; set; }
        public bool Estado { get; set; }
        public int UsuarioAprobadoId { get; set; } 
        public DateTime FechaAprobado { get; set; }
    }
}