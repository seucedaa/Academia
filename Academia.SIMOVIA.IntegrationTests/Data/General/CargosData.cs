using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
