using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    [ExcludeFromCodeCoverage]
    public class RegistrarViajePorSolicitudDto
    {
        public int SolicitudId { get; set; }
        public ViajeDto Viaje { get; set; }
    }
}
