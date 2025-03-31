using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje
{
    [ExcludeFromCodeCoverage]
    public class EstadosSolicitudes
    {
        public EstadosSolicitudes()
        {
            Descripcion = string.Empty;
            Estado = true;
            UsuarioCreacion = new Usuarios();
            UsuarioModificacion = new Usuarios();

            Solicitudes = new HashSet<Solicitudes>();
        }

        public int EstadoSolicitudId { get; set; }
        public string Descripcion { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Estado { get; set; }

        public virtual Usuarios UsuarioCreacion { get; set; }
        public virtual Usuarios UsuarioModificacion { get; set; }
        public virtual ICollection<Solicitudes> Solicitudes { get; set; }
    }
}
