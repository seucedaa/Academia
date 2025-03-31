namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso
{
    public class Roles
    {
        public Roles()
        {
            Descripcion = string.Empty;
            Estado = true;

            PantallasPorRoles = new HashSet<PantallasPorRoles>();
            Usuarios = new HashSet<Usuarios>();
        }
        public int RolId { get; set; }
        public string Descripcion { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Estado { get; set; }

        public virtual Usuarios? UsuarioCreacion { get; set; }
        public virtual Usuarios? UsuarioModificacion { get; set; }
        public virtual ICollection<PantallasPorRoles>? PantallasPorRoles { get; set; }
        public virtual ICollection<Usuarios>? Usuarios { get; set; }

    }
}
