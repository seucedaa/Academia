using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje
{
    public class Puntuaciones
    {
        public Puntuaciones()
        {
            Descripcion = string.Empty;
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
