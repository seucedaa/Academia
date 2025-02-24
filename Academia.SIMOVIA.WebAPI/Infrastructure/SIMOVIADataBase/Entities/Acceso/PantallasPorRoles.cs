namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso
{
    public class PantallasPorRoles
    {
        public int PantallaPorRolId { get; set; }
        public int PantallaId { get; set; }
        public int RolId { get; set; }

        public virtual Pantallas Pantalla { get; set; }
        public virtual Roles Rol { get; set; }
    }
}
