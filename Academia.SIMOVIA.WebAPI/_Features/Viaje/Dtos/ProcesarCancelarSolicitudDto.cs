using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ProcesarCancelarSolicitudDto
    {
        public int SolicitudId { get; set; }
        public int UsuarioAprobadoId { get; set; }
    }

}
