using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;

namespace Academia.SIMOVIA.IntegrationTests.Data.Viaje
{
    public static class ColaboradoresPorSucursalData
    {
        public static ColaboradoresPorSucursal AsignacionColaboradorSucursal => new()
        {
            ColaboradorId = 1,
            SucursalId = 1
        };
    }
}
