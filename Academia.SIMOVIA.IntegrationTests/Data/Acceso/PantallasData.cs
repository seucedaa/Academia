using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;

namespace Academia.SIMOVIA.IntegrationTests.Data.Acceso
{
    public static class PantallasData
    {
        public static Pantallas PantallaDashboard => new()
        {
            PantallaId = 1,
            Descripcion = "Dashboard",
            DireccionURL = "url",
            UsuarioCreacionId = 1,
            FechaCreacion = DateTime.Now
        };
    }
}
