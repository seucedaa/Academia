using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
