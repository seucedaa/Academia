using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje
{
    public class Solicitudes
    {
        public Solicitudes()
        {
            Descripcion = string.Empty;
            Estado = true;

            Notificaciones = new HashSet<Notificaciones>();
        }

        public int SolicitudId { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioId { get; set; }
        public int? ViajeEncabezadoId { get; set; }
        public int EstadoSolicitudId { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Estado { get; set; }

        public virtual Usuarios Usuario { get; set; }
        public virtual Usuarios UsuarioModificacion { get; set; }
        public virtual ViajesEncabezado ViajeEncabezado { get; set; }
        public virtual EstadosSolicitudes EstadoSolicitud { get; set; }
        public virtual ICollection<Notificaciones> Notificaciones { get;set; } 
    }
}
