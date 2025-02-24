using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General
{
    public class EstadosCiviles
    {
        public EstadosCiviles() { 
            Descripcion = string.Empty;
            Estado = true;

            Colaboradores = new HashSet<Colaboradores>();  
        }
        public int EstadoCivilId { get; set; }
        public string Descripcion { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Estado { get; set; }

        public virtual Usuarios UsuarioCreacion { get; set; }
        public virtual Usuarios UsuarioModificacion{ get; set; }
        public virtual ICollection<Colaboradores> Colaboradores { get; set; }
    }
}
