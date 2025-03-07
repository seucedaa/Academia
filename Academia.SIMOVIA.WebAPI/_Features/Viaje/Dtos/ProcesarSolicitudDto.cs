using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ProcesarSolicitudDto
    {
        public int SolicitudId { get; set; }
        public DateTime FechaViaje { get; set; }

        public int? ViajeEncabezadoId { get; set; }
        public int EstadoSolicitudId { get; set; }
        public int? UsuarioAprobadoId { get; set; }
        public DateTime? FechaAprobado { get; set; }
    }
}
