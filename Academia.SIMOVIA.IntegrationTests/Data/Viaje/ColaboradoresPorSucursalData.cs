using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
