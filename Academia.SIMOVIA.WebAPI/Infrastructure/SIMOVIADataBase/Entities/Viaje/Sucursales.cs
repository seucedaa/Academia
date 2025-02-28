using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje
{
    public class Sucursales
    {
        public Sucursales()
        {
            Descripcion = string.Empty;
            Telefono = string.Empty;
            DireccionExacta = string.Empty;
            Estado = true;

            ColaboradoresPorSucursal = new HashSet<ColaboradoresPorSucursal>();
            Viajes = new HashSet<ViajesEncabezado>();
            Solicitudes = new HashSet<Solicitudes>();
        }

        public int SucursalId { get; set; }
        public string Descripcion { get; set; }
        public string Telefono { get; set; }
        public string DireccionExacta { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public int CiudadId { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Estado { get; set; }

        public virtual Ciudades Ciudad { get; set; }
        public virtual Usuarios UsuarioCreacion { get; set; }
        public virtual Usuarios UsuarioModificacion { get; set; }
        public ICollection<ColaboradoresPorSucursal> ColaboradoresPorSucursal { get; set; }
        public virtual ICollection<ViajesEncabezado> Viajes { get; set; }
        public virtual ICollection<Solicitudes> Solicitudes { get; set; }

    }
}
