using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;

namespace Academia.SIMOVIA.IntegrationTests.Data.Acceso
{
    public static class PantallasPorRolesData
    {
        public static PantallasPorRoles PantallaAsignada => new()
        {
            RolId = 1,
            PantallaId = 1
        };
    }
}
