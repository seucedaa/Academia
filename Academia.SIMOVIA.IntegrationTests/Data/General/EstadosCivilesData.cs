using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
