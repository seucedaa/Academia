using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;

namespace Academia.SIMOVIA.IntegrationTests.Data.Viaje
{
    public static class SucursalesData
    {
        public static Sucursales SucursalPrueba => new()
        {
            SucursalId = 1,
            Descripcion = "Sucursal 1",
            UsuarioCreacionId = 1,
            FechaCreacion = DateTime.Now
        };
    }
}
