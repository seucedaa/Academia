using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;

namespace Academia.SIMOVIA.IntegrationTests.Data.General
{
    public class CargosData
    {
        public static Cargos CargoPrueba => new()
        {
            CargoId = 1,
            Descripcion = "prueba",
            UsuarioCreacionId = 1,
            FechaCreacion = DateTime.Now
        };
    }
}
