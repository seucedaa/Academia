using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;

namespace Academia.SIMOVIA.IntegrationTests.Data.General
{
    public static class CiudadesData
    {
        public static Ciudades CiudadPrueba => new()
        {
            CiudadId = 1,
            Descripcion = "Ciudad Central",
            UsuarioCreacionId = 1,
            FechaCreacion = DateTime.Now
        };
    }
}
