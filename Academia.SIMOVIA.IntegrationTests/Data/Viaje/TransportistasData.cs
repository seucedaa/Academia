﻿using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academia.SIMOVIA.IntegrationTests.Data.Viaje
{
    public class TransportistasData
    {
        public static Transportistas TransportistaPrueba => new()
        {
            TransportistaId = 1,
            Nombres = "Transportista 1",
            Apellidos = "Transportista 1",
            Tarifa = 10m,
            UsuarioCreacionId = 1,
            FechaCreacion = DateTime.Now
        };
    }
}
