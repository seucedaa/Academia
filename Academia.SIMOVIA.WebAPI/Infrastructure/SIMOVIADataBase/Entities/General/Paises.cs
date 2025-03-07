using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General
{
    [ExcludeFromCodeCoverage]
    public class Paises
    {
        public Paises()
        {
            Codigo = string.Empty;
            Descripcion = string.Empty;
            CodigoTelefonico = string.Empty;

            MonedasPorPais = new HashSet<MonedasPorPais>();
            Estados = new HashSet<Estados>();
        }

        public int PaisId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string CodigoTelefonico { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual Usuarios UsuarioCreacion { get; set; }
        public virtual Usuarios UsuarioModificacion { get; set; }
        public virtual ICollection<MonedasPorPais> MonedasPorPais { get; set; }
        public virtual ICollection<Estados> Estados { get; set; } 
    }
}
