using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso
{
    [ExcludeFromCodeCoverage]
    public class Pantallas
    {
        public Pantallas()
        {
            Descripcion = string.Empty;
            DireccionURL = string.Empty;
            UsuarioCreacion = new Usuarios();
            UsuarioModificacion = new Usuarios();
            PantallasPorRoles = new HashSet<PantallasPorRoles>();
        }

        public int PantallaId { get; set; }
        public string Descripcion { get; set; }
        public string DireccionURL { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Estado { get; set; }

        public virtual Usuarios UsuarioCreacion { get; set; }
        public virtual Usuarios UsuarioModificacion { get; set; }
        public virtual ICollection<PantallasPorRoles> PantallasPorRoles { get; set; }
    }
}
