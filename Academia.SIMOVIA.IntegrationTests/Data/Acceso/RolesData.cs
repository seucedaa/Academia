using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;

namespace Academia.SIMOVIA.IntegrationTests.Data.Acceso
{
    public static class RolesData
    {
        public static Roles RolPrueba => new()
        {
            RolId = 1,
            Descripcion = "prueba",
            UsuarioCreacionId = 1,
            FechaCreacion = DateTime.Now
        };
    }
}
