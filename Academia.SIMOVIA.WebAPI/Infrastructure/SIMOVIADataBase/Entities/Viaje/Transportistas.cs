using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje
{
    public class Transportistas
    {
        public Transportistas()
        {
            DNI = string.Empty;
            Nombres = string.Empty;
            Apellidos = string.Empty;
            Telefono = string.Empty;
            Estado = true;

            Viajes = new HashSet<ViajesEncabezado>();
        }

        public int TransportistaId { get; set; }
        public string DNI { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public decimal Tarifa { get; set; }
        public int CiudadId { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Estado { get; set; }

        public virtual Ciudades Ciudad { get; set; }
        public virtual Usuarios UsuarioCreacion { get; set; }
        public virtual Usuarios UsuarioModificacion { get; set; }
        public virtual ICollection<ViajesEncabezado> Viajes { get; set; }
    }
}
