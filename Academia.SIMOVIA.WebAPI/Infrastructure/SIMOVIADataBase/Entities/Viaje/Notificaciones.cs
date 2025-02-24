using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje
{
    public class Notificaciones
    {
        public Notificaciones()
        {
            Titulo = string.Empty;
            Descripcion = string.Empty;
        }

        public int NotificacionId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public int ReceptorId { get; set; }
        public int SolicitudId { get; set; }

        public virtual Usuarios Receptor { get; set; }
        public virtual Solicitudes Solicitud { get; set; }
    }
}
