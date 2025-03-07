using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academia.SIMOVIA.IntegrationTests.Data.General
{
    public static class ColaboradoresData
    {
        public static Colaboradores ColaboradorPrueba => new()
        {
            ColaboradorId = 1,
            DNI = "1234567890123",
            Nombres = "sua",
            Apellidos = "sua",
            CorreoElectronico = "sua@gmail.com",
            Telefono = "12345678",
            Sexo = "M",
            FechaNacimiento = DateTime.Now,
            DireccionExacta = "los angeles",
            Latitud = 15.25m,
            Longitud = -88.235m,
            EstadoCivilId = 1,
            CargoId = 1,
            CiudadId = 1,
            UsuarioCreacionId = 1,
            FechaCreacion = DateTime.Now
        };
    }
}
