using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje
{
    [ExcludeFromCodeCoverage]
    public class Solicitudes
    {
        public Solicitudes()
        {
            Descripcion = string.Empty;
            Estado = true;
            Usuario = new Usuarios();
            UsuarioAprobado = new Usuarios();
            ViajeEncabezado = new ViajesEncabezado();
            Sucursal = new Sucursales();
            EstadoSolicitud = new EstadosSolicitudes();

            Notificaciones = new HashSet<Notificaciones>();
        }

        public int SolicitudId { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioId { get; set; }
        public int? ViajeEncabezadoId { get; set; }
        public int SucursalId { get; set; }
        public int EstadoSolicitudId { get; set; }
        public int? UsuarioAprobadoId { get; set; }
        public DateTime? FechaAprobado { get; set; }
        public bool Estado { get; set; }
        public bool AgregarViajeSiguiente { get; set; }
        public DateTime FechaViaje { get; set; }

        public virtual Usuarios Usuario { get; set; }
        public virtual Usuarios UsuarioAprobado { get; set; }
        public virtual ViajesEncabezado ViajeEncabezado { get; set; }
        public virtual Sucursales Sucursal { get; set; }
        public virtual EstadosSolicitudes EstadoSolicitud { get; set; }
        public virtual ICollection<Notificaciones> Notificaciones { get; set; }
    }
}
