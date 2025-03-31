using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    [ExcludeFromCodeCoverage]
    public class RegistrarViajePorSolicitudDto
    {
        public RegistrarViajePorSolicitudDto()
        {
            Viaje = new ViajeDto();
        }
        public int SolicitudId { get; set; }
        public ViajeDto Viaje { get; set; }
    }
}
