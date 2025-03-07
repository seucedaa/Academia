using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General
{
    [ExcludeFromCodeCoverage]
    public class Cargos
    {
        public Cargos()
        {
            Descripcion = string.Empty;
            Estado = true;

            Colaboradores = new HashSet<Colaboradores>();
        }

        public int CargoId { get; set; }
        public string Descripcion { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Estado { get; set; }

        public virtual Usuarios UsuarioCreacion { get; set; }
        public virtual Usuarios UsuarioModificacion { get; set; }
        public virtual ICollection<Colaboradores> Colaboradores { get; set; }
    }
}
