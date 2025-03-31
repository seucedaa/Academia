using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso
{
    [ExcludeFromCodeCoverage]
    public class PantallasPorRoles
    {
        public PantallasPorRoles()
        {
            Pantalla = new Pantallas();
            Rol = new Roles();
        }
        public int PantallaPorRolId { get; set; }
        public int PantallaId { get; set; }
        public int RolId { get; set; }

        public virtual Pantallas Pantalla { get; set; }
        public virtual Roles Rol { get; set; }
    }
}
