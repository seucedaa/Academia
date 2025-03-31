using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;

namespace Academia.SIMOVIA.IntegrationTests.Data.General
{
    public static class EstadosCivilesData
    {
        public static EstadosCiviles EstadoPrueba => new()
        {
            EstadoCivilId = 1,
            Descripcion = "Soltero",
            UsuarioCreacionId = 1,
            FechaCreacion = DateTime.Now
        };
    }
}
