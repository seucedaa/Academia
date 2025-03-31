using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General
{
    [ExcludeFromCodeCoverage]
    public class Ciudades
    {
        public Ciudades()
        {
            Codigo = string.Empty;
            Descripcion = string.Empty;

            Estado = new Estados();
            UsuarioCreacion = new Usuarios();

            Colaboradores = new HashSet<Colaboradores>();
            Sucursales = new HashSet<Sucursales>();
            Transportistas = new HashSet<Transportistas>();
        }

        public int CiudadId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int EstadoId { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual Estados Estado { get; set; }
        public virtual Usuarios UsuarioCreacion { get; set; }
        public virtual Usuarios? UsuarioModificacion { get; set; }
        public virtual ICollection<Colaboradores> Colaboradores { get; set; }
        public virtual ICollection<Sucursales> Sucursales { get; set; }
        public virtual ICollection<Transportistas> Transportistas { get; set; }
    }
}
