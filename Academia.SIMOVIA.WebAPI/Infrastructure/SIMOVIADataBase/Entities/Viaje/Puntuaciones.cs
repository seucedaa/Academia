using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje
{
    [ExcludeFromCodeCoverage]
    public class Puntuaciones
    {
        public Puntuaciones()
        {
            Descripcion = string.Empty;
            ViajeEncabezado = new ViajesEncabezado();
            UsuarioCreacion = new Usuarios();
            UsuarioModificacion = new Usuarios();
        }

        public int PuntuacionId { get; set; }
        public int Cantidad { get; set; }
        public string? Descripcion { get; set; }
        public int ViajeEncabezadoId { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual ViajesEncabezado ViajeEncabezado { get; set; }
        public virtual Usuarios UsuarioCreacion { get; set; }
        public virtual Usuarios UsuarioModificacion { get; set; }
    }
}
