using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    [ExcludeFromCodeCoverage]
    public class SolicitudDto
    {
        public SolicitudDto()
        {
            Descripcion = string.Empty;
        }
        public string Descripcion { get; set; }
        public int UsuarioId { get; set; }
        public int? ViajeEncabezadoId { get; set; }
        public int SucursalId { get; set; }
        public bool AgregarViajeSiguiente { get; set; }
        public DateTime FechaViaje { get; set; }
    }
}
