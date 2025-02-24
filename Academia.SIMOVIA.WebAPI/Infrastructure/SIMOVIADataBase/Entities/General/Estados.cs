using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General
{
    public class Estados
    {
        public Estados()
        {
            Codigo = string.Empty;
            Descripcion = string.Empty;

            Ciudades = new HashSet<Ciudades>();
        }

        public int EstadoId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int PaisId { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual Paises Pais { get; set; }
        public virtual Usuarios UsuarioCreacion { get; set; }
        public virtual Usuarios UsuarioModificacion { get; set; }
        public virtual ICollection<Ciudades> Ciudades { get; set; }
    }
}
